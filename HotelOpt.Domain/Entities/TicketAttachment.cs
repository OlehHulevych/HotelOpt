using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class TicketAttachment:BaseEntity
{
    public Guid TicketId { get; private set; }
    public MaintenanceTicket Ticket { get; private set; }
    public string FileName { get; private set; }
    public string BlobUrl { get; private set; }
    public Guid UploadedById { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }

    private TicketAttachment()
    {
        FileName = null!;
        Ticket = null!;
       
    }
    
    public TicketAttachment(Guid ticketId, string fileName, string blobUrl, Guid uploadedById, Guid tenantId)
    {
        TicketId = ticketId;
        FileName = fileName;
        BlobUrl = blobUrl;
        UploadedById = uploadedById;
        UploadedAt = DateTimeOffset.UtcNow;
        TenantId = tenantId;
    }
}