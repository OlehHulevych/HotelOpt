using System.Linq.Expressions;
using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Services;

public class ShiftService(
    ICurrentTenantService currentTenantService,
    IRepository<Shift> repository,
    IIdentityService identityService,
    IRepository<MaintenanceTicket> ticketRepository,
    IRepository<HouseKeepingTask> taskRepository,
    IRepository<Room> roomRepository)
    : IShiftService
{
    public async Task<bool> AddShift(CreateShiftDto dto)
    {
        Shift shift = new Shift(dto.StaffId, dto.PropertyId,currentTenantService.TenantId, dto.StartTime, dto.EndTime);
        var result = await repository.Add(shift);
        return result;
    }

    public async Task UpdateShift(UpdateShiftDto dto)
    {
        Shift shift = await repository.GetById(dto.Id);
        shift.Update(dto.StartTime,dto.EndTime,dto.StaffId,dto.Status);
        await repository.Update(shift);
    }

    public async Task<PaginatedResult<ShiftDto>> GetAllShifts(int currentPage, int pageSize)
    {
        (var shifts, int totalCount) = await repository.GetAllPaginated( currentPage,  pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<PaginatedResult<ShiftDto>> GetAllShiftsByProperty(Guid id, int currentPage, int pageSize)
    {
        var (shifts, totalCount) = await repository.GetByConditionPaginated(e=>e.PropertyId == id, currentPage, pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId,"Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<ShiftDto> GetShiftById(Guid id)
    {
        var shift = await repository.GetById(id);
        var ids = new List<Guid> { shift.StaffId};
        var names = await identityService.GetUserNamesByIds(ids);
        var shiftDto = new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId,shift.PropertyId,shift.StaffId,names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status);
        return shiftDto;
    }

    public async Task<PaginatedResult<ShiftDto>> GetShiftByStaff(Guid id, int currentPage, int pageSize)
    {
        (List<Shift> shifts, int totalCount) = await repository.GetByConditionPaginated(e=>e.StaffId == id, currentPage, pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<bool> DeleteShift(Guid id)
    {
        var result = await repository.Delete(id);
        return result;
    }

    public async  Task<ShiftReportDto> GetReportAsync(Guid shiftId)
    {
        Shift shift = await repository.GetById(shiftId);
        Expression<Func<HouseKeepingTask, bool>> taskConditions = t => (t.AssignedToId == shift.StaffId) 
                                                                   && (t.Status==HouseKeepingTaskStatus.Completed)
                                                                   && (t.ScheduledAt>=shift.StartTime)
                                                                   && (t.CompletedAt <= shift.EndTime);
        List<HouseKeepingTask> houseKeepingTasks = await taskRepository.GetByCondition(taskConditions );
        var roomIds = houseKeepingTasks.Select(t => t.RoomId).Distinct().ToList();
        var rooms = await roomRepository.GetByCondition(r=>roomIds.Contains(r.Id));
        var roomNumbers = rooms.ToDictionary(r => r.Id, r => r.RoomNumber);
        var names = await identityService.GetUserNamesByIds([shift.StaffId]);
        var assignedByNames = await identityService.GetUserNamesByIds(houseKeepingTasks.Select(t=>t.AssignedById));
       
        List<HouseKeepingTaskDto> taskList = houseKeepingTasks.Select(t => new HouseKeepingTaskDto(t.Id,t.Title,t.AssignedToId,names.GetValueOrDefault(t.AssignedToId, "Unknown"), 
            t.AssignedById,assignedByNames.GetValueOrDefault(t.AssignedById, "Unknown"),t.RoomId, roomNumbers.GetValueOrDefault(t.RoomId, "Unknown"),t.Status,t.ScheduledAt,t.CompletedAt)).ToList();
        Expression<Func<MaintenanceTicket, bool>> ticketConditions = t => (t.StaffId == shift.StaffId) 
                                                                       && (t.Status==TicketStatus.Resolved)
                                                                       &&(t.ResolvedAt >= shift.StartTime)
                                                                       && (t.ResolvedAt < shift.EndTime);
        List<MaintenanceTicket> tickets = await ticketRepository.GetByCondition(ticketConditions);
        var ticketNames = await identityService.GetUserNamesByIds(tickets.Select(t => t.StaffId).Concat(tickets.Select(t=>t.ReportedId)));
        List<MaintenanceTicketDto> ticketList = tickets.Select(t=>new MaintenanceTicketDto(t.Id,t.Title,t.Description,t.StaffId,ticketNames.GetValueOrDefault(t.StaffId,"Unknown"),t.ReportedId,ticketNames.GetValueOrDefault(t.ReportedId,"Unknown"),t.RoomId,roomNumbers.GetValueOrDefault(t.RoomId, "Unknown"),t.PropertyId,t.Priority,t.Status,t.ResolvedAt)).ToList();
        return new ShiftReportDto(shift.StaffId,names.GetValueOrDefault(shift.StaffId,"Unknown"),shift.StartTime,shift.EndTime,shift.PropertyId,taskList.Count, taskList,ticketList.Count,ticketList );
    }
}