using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class Invoice:BaseEntity
{
    public Guid BookingId { get; private set; }
    public Booking Booking { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public string GuestName { get; private set; }
    public DateTimeOffset CheckInDate { get; private set; }
    public DateTimeOffset CheckOutDate { get; private set; }
    public int Nights { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal PricePerNight { get; private set; }
    public DateTimeOffset IssuedAt { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }

    private Invoice()
    {
        Booking = null!;
        Room = null!;
        GuestName = null!;
        CheckInDate = DateTimeOffset.MinValue;
        CheckOutDate = DateTimeOffset.MinValue;
        Nights = 0;
        TotalAmount = 0;
        PricePerNight = 0;
        IssuedAt = DateTimeOffset.UtcNow;
        Status = InvoiceStatus.Draft;
        Tenant = null!;
    }

    public Invoice(Guid bookingId, Guid roomId, string guestName, DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,  Guid tenantId, decimal pricePerNight)
    {
        BookingId = bookingId;
        RoomId = roomId;
        GuestName = guestName;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        TenantId = tenantId;
        Nights =(int) Math.Floor((checkOutDate - checkInDate).TotalDays);
        IssuedAt = DateTimeOffset.UtcNow;
        Status = InvoiceStatus.Draft;
        TotalAmount = pricePerNight * Nights;
    }
}