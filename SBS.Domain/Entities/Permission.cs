namespace SBS.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty; // e.g., "Overview", "Manage_Booking"
    public int FunctionId { get; set; } // Matching ServiceFunction.ts in Frontend
    public string Description { get; set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
