using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class RoomPhoto:BaseEntity
{
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public Guid UploadedById { get; private set; }
    public string Url { get; private set; }

    private RoomPhoto()
    {
        Room = null!;
        Tenant = null!;
        Url = null!;
    }

    public RoomPhoto(Guid roomId, Guid tenantId, Guid uploadedById, string url)
    {
        RoomId = roomId;
        TenantId = tenantId;
        UploadedById = uploadedById;
        Url = url;
    }
    
}