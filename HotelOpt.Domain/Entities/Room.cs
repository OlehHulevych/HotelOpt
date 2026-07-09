using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class Room:BaseEntity
{
    public string RoomNumber { get; private set; }
    public string Description { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public RoomStatus Status { get; private set; }
    public RoomType Type { get; private set; }
    
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public ICollection<RoomPhoto> Photos { get; private set; }

    private Room()
    {
        RoomNumber = null!;
        Description = null!;
        Property = null!;
        Type = RoomType.Single;
        Tenant = null!;
    }

    public Room(string roomNumber, string description, Guid propertyId, RoomType type, Guid tenantId)
    {
        RoomNumber = roomNumber;
        Description = description;
        PropertyId = propertyId;
        Type = type;
        Status = RoomStatus.Cleaning;
        TenantId = tenantId;
    }

    

    public void Update(string? roomNumber, string? description, Guid? propertyId, RoomType? type, RoomStatus? status)
    {
        RoomNumber = roomNumber ?? RoomNumber;
        Description = description ?? Description;
        PropertyId = propertyId ?? PropertyId;
        Type = type ?? Type;
        Status = status ?? Status;
    }

    public void SetOccupied()
    {
        if (Status != RoomStatus.Available) throw new Exception("The room is not available");
            Status = RoomStatus.Occupied;
    }

    public void SetCleaning()
    {
        if (Status != RoomStatus.Occupied) throw new Exception("The room is not occupied ");
        Status = RoomStatus.Cleaning;
    }

    public void SetAvailable()
    {
        if (Status != RoomStatus.Cleaning || Status != RoomStatus.Maintenance) throw new Exception("The room is not cleaning or under maintenance");
        Status = RoomStatus.Available;
    }

    public void SetMaintenance()
    {
        if (Status != RoomStatus.Available || Status != RoomStatus.Occupied) throw new Exception("The room is not available for maintenance");
        Status = RoomStatus.Maintenance;
    }

    
}