using SBS.Domain.Enums;

namespace SBS.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
