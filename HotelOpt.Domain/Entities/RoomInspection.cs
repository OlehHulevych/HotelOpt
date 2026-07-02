using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class RoomInspection:BaseEntity
{
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public string PhotoUrl { get; private set; }
    public bool Passed { get; private set; }
    public string? Issues { get; private set; }
    public Guid InspectedById { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public DateTimeOffset InspectedAt { get; private set; }


    private RoomInspection()
    {
        Room = null!;
        Tenant = null!;
        PhotoUrl = null!;
        Property = null!;
        InspectedAt = DateTimeOffset.UtcNow;
        Passed = false;
        Issues = null;
    }

    public RoomInspection(Guid roomId, Guid tenantId, Guid inspectedById, Guid propertyId, string? issues, string photoUrl, bool passed)
    {
        RoomId = roomId;
        TenantId = tenantId;
        InspectedById = inspectedById;
        PropertyId = propertyId;
        Issues = issues;
        PhotoUrl = photoUrl;
        InspectedAt = DateTimeOffset.UtcNow;
        Passed = passed;
    }
    
}