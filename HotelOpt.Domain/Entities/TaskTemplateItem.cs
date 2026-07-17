using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class TaskTemplateItem:BaseEntity
{
    public Guid TemplateId { get; private set; }
    public TaskTemplate TaskTemplate { get; private set; }
    public string Title { get; private set; }
    public int Order { get;private set; }

    private TaskTemplateItem()
    {
        TaskTemplate = null!;
        Title = null!;
        Order = 0;
    }

    public TaskTemplateItem(Guid templateId, string title, int order)
    {
        TemplateId = templateId;
        Title = title;
        Order = order;
    }
}