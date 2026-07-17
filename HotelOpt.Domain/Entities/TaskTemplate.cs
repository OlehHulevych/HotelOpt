using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;

public class TaskTemplate:BaseEntity
{
    public string Name { get; private set; }
    public RoomType Type { get; private set; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public Guid TenantId { get;private set; }
    public Tenant Tenant { get; private set; }
    public IList<TaskTemplateItem> Items { get; private set; }

    private TaskTemplate()
    {
        Name = null!;
        Property = null!;
        Tenant = null!;
        Items = new List<TaskTemplateItem>();
    }

    public TaskTemplate(string name, RoomType type, Guid propertyId, Guid tenantId)
    {
        Name = name;
        Type = type;
        PropertyId = propertyId;
        TenantId = tenantId;
        Items = new List<TaskTemplateItem>();

    }

    public void AddItem(TaskTemplateItem item)
    {
        Items.Add(item);
    }
}