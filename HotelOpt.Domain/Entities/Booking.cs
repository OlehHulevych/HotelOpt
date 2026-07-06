using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class Booking:BaseEntity
{
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public Guid PropertyId { get; private set; }
    public DateTimeOffset CheckInDate { get; private set; }
    public DateTimeOffset CheckOutDate { get; private set; }
    public Property Property { get; private set; }
    public BookingStatus Status { get; private set; }
    
    public ICollection<Guest> Guests { get; private set; }

    private Booking()
    {
        Guests = new List<Guest>();
        Room = null!;
        Tenant = null!;
        Property = null!;
        
    }

    public Booking( Guid roomId, Guid propertyId, Guid tenantId, DateTimeOffset checkIn,
        DateTimeOffset checkOut, Guest guest)
    {
        RoomId = roomId;
        PropertyId = propertyId;
        TenantId = tenantId;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;
        Status = BookingStatus.Confirmed;
        Guests = new List<Guest>(){guest};
       


    }

    public void AddGuest(Guest guest)
    {
        if (Guests.Any(g=>g.Id== guest.Id)) throw new Exception("Guest already have this booking");
        Guests.Add(guest);
    }

    public void CheckIn()
    {
        if (Status != BookingStatus.Confirmed) throw new Exception("The reservation is not confirmed");
        Status = BookingStatus.CheckedIn;
    }

    public void CheckOut()
    {
        if (Status != BookingStatus.CheckedIn) throw new Exception("The reservation is not checked in");
        Status = BookingStatus.CheckedOut;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.CheckedOut) throw new Exception("The booking is already checked out");
        Status = BookingStatus.Cancelled;
    }

   
    
}