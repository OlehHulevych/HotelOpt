
using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class HouseKeepingTask:BaseEntity
{
    public string Title { get; private set; }
    public Guid AssignedToId { get; private set; }
    public Guid AssignedById { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public HouseKeepingTaskStatus Status { get; private set; }
    public DateTimeOffset ScheduledAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }

    private HouseKeepingTask()
    {
        Title = null!;
        AssignedById = Guid.Empty;
        AssignedToId = Guid.Empty;
        Room = null!;
        ScheduledAt = DateTimeOffset.UtcNow;
        Tenant = null!;
    }

    public HouseKeepingTask(string title, Guid assignedById, Guid assignedToId, Guid roomId, DateTimeOffset scheduledAt, Guid tenantId)
    {
        Title = title;
        AssignedById = assignedById;
        AssignedToId = assignedToId;
        RoomId = roomId;
        Status = HouseKeepingTaskStatus.Pending;
        ScheduledAt = scheduledAt;
        TenantId = tenantId;

    }

    public void UpdateHouseKeepingTask(string? title, Guid? assignedToId)
    {
        Title = title ?? Title;
        AssignedToId = assignedToId ?? AssignedToId;
        ScheduledAt = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        
        if(Status != HouseKeepingTaskStatus.InProgress)
            throw new InvalidOperationException("Failed to complete the task");

        Status = HouseKeepingTaskStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;

    }
    public void Start()
    {
        if (Status != HouseKeepingTaskStatus.Pending)  throw new InvalidOperationException("Failed to start task");
        Status = HouseKeepingTaskStatus.InProgress;
        
        
    }

    public void Cancel()
    {
        if( Status==HouseKeepingTaskStatus.Completed || Status==HouseKeepingTaskStatus.Cancelled ) throw new InvalidOperationException("Failed to cancel task");
        Status = HouseKeepingTaskStatus.Cancelled;
        
    }
    public void Reassign(Guid newStaffId)
    {
        if (Status == HouseKeepingTaskStatus.Completed || Status == HouseKeepingTaskStatus.Cancelled)
            throw new InvalidOperationException("Cannot reassign a completed or cancelled task");
        AssignedToId = newStaffId;
    }
    
    
    
}