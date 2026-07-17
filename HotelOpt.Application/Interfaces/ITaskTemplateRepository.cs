using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Interfaces;

public interface ITaskTemplateRepository : IRepository<TaskTemplate>
{
    Task<List<TaskTemplate>> GetByPropertyWithItemsAsync(Guid propertyId);
    Task<TaskTemplate> GetByIdWithItemsAsync(Guid id);
}
