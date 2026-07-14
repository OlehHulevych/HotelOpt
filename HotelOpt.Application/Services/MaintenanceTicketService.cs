using System.Linq.Expressions;
using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Services;

public class MaintenanceTicketService:IMaintenanceTicketService
{
    private readonly IRepository<MaintenanceTicket> _repository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IIdentityService _identityService;
    private readonly IRepository<Room> _roomRepository;

    public MaintenanceTicketService(IRepository<MaintenanceTicket> repository, ICurrentTenantService currentTenantService, IIdentityService identityService, IRepository<Room> roomRepository)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _identityService = identityService;
        _roomRepository = roomRepository;
    }
    public async Task<bool> AddTicket(CreateMaintenanceTicketDto dto)
    {
        MaintenanceTicket newTicket = new MaintenanceTicket(dto.Title,dto.Description,dto.StaffId, dto.ReportedId, dto.Priority,dto.RoomId,dto.PropertyId,_currentTenantService.TenantId);
        var room = await _roomRepository.GetById(dto.RoomId);
        room.SetMaintenance();
        await _roomRepository.Update(room);
        var result = await _repository.Add(newTicket);
        return result;
    }

    public async Task UpdateTask(UpdateMaintenanceTicketDto dto)
    {
        MaintenanceTicket ticketForUpdating = await _repository.GetById(dto.Id);
        ticketForUpdating.Update(dto.title, dto.description, dto.staffId, dto.reportedId, dto.priority);
        await _repository.Update(ticketForUpdating);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAll(int currentPage, int pageSize, MaintenanceTicketFilterDto filters)
    {
        Expression<Func<MaintenanceTicket, bool>> predicate = t =>
            (!filters.Status.HasValue || filters.Status == t.Status) &&
            (!filters.Priority.HasValue || filters.Priority == t.Priority) &&
            (!filters.CreatedFrom.HasValue || filters.CreatedFrom <= t.CreatedAt) &&
            (!filters.CreatedTo.HasValue || filters.CreatedTo >= t.CreatedAt);
        
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated(predicate,currentPage,pageSize);
        var idsStaff =  query.Select(t=>t.StaffId).ToList();
        var idsReported = query.Select(t => t.ReportedId).ToList();
        var ids = idsStaff.Concat(idsReported);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,names.GetValueOrDefault(t.StaffId, "Unknown"), t.ReportedId, names.GetValueOrDefault(t.ReportedId, "Unknown"),t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAllByProperty(Guid propertyId, int currentPage, int pageSize, MaintenanceTicketFilterDto filters)
    {
        Expression<Func<MaintenanceTicket, bool>> predicate = t =>
            t.PropertyId==propertyId &&
            (!filters.Status.HasValue || filters.Status == t.Status) &&
            (!filters.Priority.HasValue || filters.Priority == t.Priority) &&
            (!filters.CreatedFrom.HasValue || filters.CreatedFrom <= t.CreatedAt) &&
            (!filters.CreatedTo.HasValue || filters.CreatedTo >= t.CreatedAt);
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated(predicate,currentPage, pageSize);
        var idsStaff =  query.Select(t=>t.StaffId).ToList();
        var idsReported = query.Select(t => t.ReportedId).ToList();
        var ids = idsStaff.Concat(idsReported);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,names.GetValueOrDefault(t.StaffId, "Unknown"), t.ReportedId,names.GetValueOrDefault(t.ReportedId, "Unknown"), t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetByStaffId(Guid staffId, int currentPage, int pageSize, MaintenanceTicketFilterDto filters)
    {
        Expression<Func<MaintenanceTicket, bool>> predicate = t =>
            t.StaffId == staffId &&
            (!filters.Status.HasValue || filters.Status == t.Status) &&
            (!filters.Priority.HasValue || filters.Priority == t.Priority) &&
            (!filters.CreatedFrom.HasValue || filters.CreatedFrom <= t.CreatedAt) &&
            (!filters.CreatedTo.HasValue || filters.CreatedTo >= t.CreatedAt);
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated(predicate,currentPage, pageSize);
        var idsStaff =  query.Select(t=>t.StaffId).ToList();
        var idsReported = query.Select(t => t.ReportedId).ToList();
        var ids = idsStaff.Concat(idsReported);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId, names.GetValueOrDefault(t.StaffId, "Unknown"), t.ReportedId, names.GetValueOrDefault(t.ReportedId, "Unknown"),t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<PaginatedResult<MaintenanceTicketDto>> GetAllByRoom(Guid roomId, int currentPage, int pageSize, MaintenanceTicketFilterDto filters)
    {
        Expression<Func<MaintenanceTicket, bool>> predicate = t =>
            t.RoomId == roomId &&
            (!filters.Status.HasValue || filters.Status == t.Status) &&
            (!filters.Priority.HasValue || filters.Priority == t.Priority) &&
            (!filters.CreatedFrom.HasValue || filters.CreatedFrom <= t.CreatedAt) &&
            (!filters.CreatedTo.HasValue || filters.CreatedTo >= t.CreatedAt);
        (List<MaintenanceTicket> query, int totalCount) = await _repository.GetByConditionPaginated(predicate,currentPage, pageSize);
        var idsStaff =  query.Select(t=>t.StaffId).ToList();
        var idsReported = query.Select(t => t.ReportedId).ToList();
        var ids = idsStaff.Concat(idsReported);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<MaintenanceTicketDto> list =  query.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId, names.GetValueOrDefault(t.StaffId, "Unknown"),t.ReportedId,names.GetValueOrDefault(t.ReportedId, "Unknown"), t.RoomId, t.PropertyId,t.Priority, t.Status, t.ResolvedAt)).ToList();
        return new PaginatedResult<MaintenanceTicketDto>(list, totalCount, pageSize,currentPage);
    }

    public async Task<MaintenanceTicketDto> GetById(Guid id)
    {
        MaintenanceTicket response = await _repository.GetById(id);
        var ids = new List<Guid> { response.StaffId, response.ReportedId};
        var names = await _identityService.GetUserNamesByIds(ids);
        MaintenanceTicketDto ticket = new MaintenanceTicketDto(response.Id, response.Title, response.Description,
            response.StaffId, names.GetValueOrDefault(response.StaffId, "Unknown"), response.ReportedId,names.GetValueOrDefault(response.ReportedId, "Unknown"),response.RoomId,response.PropertyId,response.Priority,response.Status,response.ResolvedAt);
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
        var room = await _roomRepository.GetById(ticket.RoomId);
        room.SetAvailable();
        ticket.Resolve();
        await _repository.Update(ticket);
        await _roomRepository.Update(room);
    }

    public async  Task Close(Guid id)
    {
        MaintenanceTicket ticket = await _repository.GetById(id);
        var room = await _roomRepository.GetById(ticket.RoomId);
        if (room.Status == RoomStatus.Maintenance)
        {
            room.SetAvailable();
            await _roomRepository.Update(room);
        }
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