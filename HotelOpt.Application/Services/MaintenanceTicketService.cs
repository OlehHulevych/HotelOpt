using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Services;

public class MaintenanceTicketService:IMaintenanceTicketService
{
    private readonly IRepository<MaintenanceTicket> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public MaintenanceTicketService(IRepository<MaintenanceTicket> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task<bool> AddTicket(CreateMaintenanceTicketDto dto)
    {
        MaintenanceTicket newTicket = new MaintenanceTicket(dto.Title,dto.Description,dto.StaffId, dto.ReportedId, dto.Priority,dto.RoomId,dto.PropertyId,_currentTenantService.TenantId);
        var result = await _repository.Add(newTicket);
        return result;
    }

    public async Task UpdateTask(UpdateMaintenanceTicketDto dto)
    {
        MaintenanceTicket ticketForUpdating = await _repository.GetById(dto.Id);
        ticketForUpdating.Update(dto.title, dto.description, dto.staffId, dto.reportedId, dto.priority);
        await _repository.Update(ticketForUpdating);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAll(int currentPage, int pageSize)
    {
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetAllPaginated(currentPage, pageSize);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,t.ReportedId, t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAllByProperty(Guid propertyId, int currentPage, int pageSize)
    {
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated((t=>t.PropertyId==propertyId),currentPage, pageSize);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,t.ReportedId, t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetByStaffId(Guid staffId, int currentPage, int pageSize)
    {
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated((t=>t.StaffId==staffId),currentPage, pageSize);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,t.ReportedId, t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAllByRoom(Guid roomId, int currentPage, int pageSize)
    {
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated((t=>t.RoomId == roomId),currentPage, pageSize);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,t.ReportedId, t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<MaintenanceTicketDto> GetById(Guid id)
    {
        MaintenanceTicket response = await _repository.GetById(id);
        MaintenanceTicketDto ticket = new MaintenanceTicketDto(response.Id, response.Title, response.Description,
            response.StaffId,response.ReportedId,response.RoomId,response.PropertyId,response.Priority,response.Status,response.ResolvedAt);
        return ticket;
    }

    public async Task<bool> DeleteTask(Guid id)
    {
        bool result = await _repository.Delete(id);
        return result;
    }

    public async Task Resolve(Guid id)
    {
        MaintenanceTicket ticket = await _repository.GetById(id);
        ticket.Resolve();
        await _repository.Update(ticket);
    }

    public async  Task Close(Guid id)
    {
        MaintenanceTicket ticket = await _repository.GetById(id);
        ticket.Close();
        await _repository.Update(ticket);

    }

    public async Task Reassign(Guid id, Guid staffId)
    {
        MaintenanceTicket ticket = await _repository.GetById(id);
        ticket.Reassign(staffId);
        await _repository.Update(ticket);

    }
}