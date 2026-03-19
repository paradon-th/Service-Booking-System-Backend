using SBS.Domain.Enums;

namespace SBS.Domain.Entities;

public class Booking : BaseEntity
{
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string Notes { get; set; } = string.Empty;
}
