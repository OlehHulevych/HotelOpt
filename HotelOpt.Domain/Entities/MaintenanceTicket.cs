using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class MaintenanceTicket:BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid StaffId { get; private set; }
    public Guid ReportedId { get; private set; }
    public TicketPriority Priority { get; private set; }
    public TicketStatus Status { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    
    

    private MaintenanceTicket()
    {
        Room = null!;
        Property = null!;
        Title = null!;
        Tenant = null!;
        Description = null!;
        Status = TicketStatus.Open;
        Priority = TicketPriority.Low;
    }

    public MaintenanceTicket(string title, string description, Guid staffId, Guid reportedId, TicketPriority priority, Guid roomId, Guid propertyId, Guid tenantId)
    {
        Title = title;
        Description = description;
        StaffId = staffId;
        ReportedId = reportedId;
        Priority = priority;
        RoomId = roomId;
        PropertyId = propertyId;
        TenantId = tenantId;
    }

    public void Update(string? title, string? description, Guid? staffId, Guid? reportedId, TicketPriority? priority)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        StaffId = staffId ?? StaffId;
        ReportedId = reportedId ?? ReportedId;
        Priority = priority ?? Priority;
    }

    public void Resolve()
    {
        if (Status != TicketStatus.Open && Status != TicketStatus.InProgress) throw new Exception("Failed to resolve task");
        Status = TicketStatus.Resolved;
        ResolvedAt = DateTimeOffset.UtcNow;
    }

    public void Close()
    {
        if (Status != TicketStatus.Resolved) throw new Exception("Failed to close the task");
        Status = TicketStatus.Closed;
    }

    public void Reassign(Guid newId)
    {
        if (Status == TicketStatus.Closed) throw new Exception("Ticket is already closed");
        StaffId = newId;
    }
}