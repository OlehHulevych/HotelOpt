using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class TaskTemplateService : ITaskTemplateService
{
    private readonly ITaskTemplateRepository _templateRepository;
    private readonly IRepository<HouseKeepingTask> _taskRepository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly ICurrentUserService _currentUserService;

    public TaskTemplateService(
        ITaskTemplateRepository templateRepository,
        IRepository<HouseKeepingTask> taskRepository,
        ICurrentTenantService currentTenantService,
        ICurrentUserService currentUserService)
    {
        _templateRepository = templateRepository;
        _taskRepository = taskRepository;
        _currentTenantService = currentTenantService;
        _currentUserService = currentUserService;
    }

    public async Task CreateAsync(CreateTaskTemplateDto dto)
    {
        var template = new TaskTemplate(dto.Name, dto.RoomType, dto.PropertyId, _currentTenantService.TenantId);
        foreach (var item in dto.Items.OrderBy(i => i.Order))
            template.AddItem(new TaskTemplateItem(template.Id, item.Title, item.Order));
        await _templateRepository.Add(template);
    }

    public async Task<List<TaskTemplateDto>> GetByPropertyAsync(Guid propertyId)
    {
        var templates = await _templateRepository.GetByPropertyWithItemsAsync(propertyId);
        return templates.Select(t => new TaskTemplateDto(
            t.Id,
            t.Name,
            t.Type,
            t.PropertyId,
            t.Items.OrderBy(i => i.Order).Select(i => new TaskTemplateItemDto(i.Id, i.Title, i.Order)).ToList()
        )).ToList();
    }

    public async Task ApplyAsync(Guid templateId, ApplyTemplateDto dto)
    {
        var template = await _templateRepository.GetByIdWithItemsAsync(templateId);
        foreach (var item in template.Items.OrderBy(i => i.Order))
        {
            var task = new HouseKeepingTask(
                item.Title,
                _currentUserService.UserId,
                dto.AssignedToId,
                dto.RoomId,
                dto.ScheduledAt,
                _currentTenantService.TenantId,
                template.PropertyId
            );
            await _taskRepository.Add(task);
        }
    }
}
