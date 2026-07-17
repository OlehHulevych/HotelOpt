using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface ITaskTemplateService
{
    Task CreateAsync(CreateTaskTemplateDto dto);
    Task<List<TaskTemplateDto>> GetByPropertyAsync(Guid propertyId);
    Task ApplyAsync(Guid templateId, ApplyTemplateDto dto);
}
