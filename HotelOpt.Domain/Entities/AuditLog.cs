using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class AuditLog:BaseEntity
{
    public string EntityName { get; private set; }
    public Guid EntityId { get; private set; }
    public EntityAction AuditAction { get; private set; }
    public Guid ChangedById { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public string Changes { get; private set; }

    private AuditLog()
    {
        EntityName = null!;
        EntityId = Guid.Empty;
        Tenant = null!;
        Changes = null!;
    }

    public AuditLog(string entityName, Guid entityId, EntityAction auditAction, Guid changedById, Guid tenantId, string changes)
    {
        EntityName = entityName;
        EntityId = entityId;
        AuditAction = auditAction;
        ChangedById = changedById;
        TenantId = tenantId;
        Changes = changes;
        
    }
}