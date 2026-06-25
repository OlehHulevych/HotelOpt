using HotelOpt.Application.DTOs;
using HoteOpt.Application.Pagination;

namespace HoteOpt.Application.Interfaces;

public interface IMaintenanceTicketService
{
    public Task<bool> AddTicket(CreateMaintenanceTicketDto dto);
    public Task UpdateTask(UpdateMaintenanceTicketDto dto);
    public Task<PaginatedResult<MaintenanceTicketDto>> GetAll(int currentPage, int pageSize);
    public Task<PaginatedResult<MaintenanceTicketDto>> GetAllByProperty(Guid propertyId, int currentPage, int pageSize);
    public Task<PaginatedResult<MaintenanceTicketDto>> GetByStaffId(Guid staffId, int currentPage, int pageSize);
    public Task<PaginatedResult<MaintenanceTicketDto>> GetAllByRoom(Guid roomId, int currentPage, int pageSize);
    public Task<MaintenanceTicketDto> GetById(Guid id);
    public Task<bool> DeleteTask(Guid id);
    public Task Resolve(Guid id);
    public Task Close(Guid id);
    public Task Reassign(Guid id, Guid staffId);
    

}