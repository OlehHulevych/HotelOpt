using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class Shift:BaseEntity
{
    public Guid StaffId { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public ShiftStatus Status { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }

    private Shift()
    {
        Property = null!;
        Tenant = null!;
        Status = ShiftStatus.Scheduled;
        
    }

    public Shift(Guid staffId, Guid propertyId, Guid tenantId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        StaffId = staffId;
        PropertyId = propertyId;
        TenantId = tenantId;
        Tenant = null!;
        Property = null!;
        StartTime = startTime;
        EndTime = endTime;
        Status = ShiftStatus.Scheduled;


    }

    public void Update( DateTimeOffset? startTime, DateTimeOffset? endTime, Guid? staffId, ShiftStatus? status)
    {
        StartTime = startTime ?? StartTime;
        EndTime = endTime ?? EndTime;
        StaffId = staffId ?? StaffId;
        Status = status ?? Status;
    }
    
    
}