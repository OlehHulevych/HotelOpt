using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class Message:BaseEntity
{
    public string Content { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    
    private Message()
    {
        Content = null!;
        Property = null!;
        Tenant = null!;
    }

    public Message(string content, Guid senderId, Guid propertyId, Guid tenantId)
    {
        Content = content;
        SenderId = senderId;
        PropertyId = propertyId;
        TenantId = tenantId;
    }

    public void Update(string? content)
    {
        Content = content ?? Content;
    }
}

