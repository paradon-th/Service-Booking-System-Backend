using Microsoft.EntityFrameworkCore;
using SBS.Domain.Entities;

namespace SBS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Service-Booking relationship
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Service)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ServiceId);

        // Configure User-Booking relationship
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        // Configure User-Role relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // Configure Role-Permission (Many-to-Many via RolePermission)
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        // Seeding Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "System Administrator" },
            new Role { Id = 2, Name = "Customer", Description = "Registered Customer" }
        );

        // Seeding Permissions (FunctionIds matching Frontend ServiceFunction.ts)
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "Overview", FunctionId = 12, Description = "Access Dashboard" },
            new Permission { Id = 2, Name = "User Management", FunctionId = 7, Description = "Manage Users" }
        );

        // Seeding RolePermissions (Admin has everything)
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
            new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
            new RolePermission { Id = 3, RoleId = 2, PermissionId = 1 } // Customer can see Overview
        );
    }
}
