namespace HotelOpt.Application.DTOs;

public class CreateBookingDto
{
    public Guid RoomId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid PrimaryGuestId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
}
