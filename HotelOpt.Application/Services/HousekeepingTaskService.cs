using System.Linq.Expressions;
using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Services;

public class HousekeepingTaskService:IHousekeepingTaskService
{
    private readonly IRepository<HouseKeepingTask> _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IIdentityService _identityService;
    private readonly IRepository<Room> _roomRepository;

    public HousekeepingTaskService(IRepository<HouseKeepingTask> repository, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService, IIdentityService identityService,IRepository<Room> roomRepository)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _currentTenantService = currentTenantService;
        _identityService = identityService;
        _roomRepository = roomRepository;
    }
    public async Task<bool> CreateTask(CreateHousekeepingTaskDto dto)
    {
        HouseKeepingTask entity = new HouseKeepingTask(dto.Title, _currentUserService.UserId, dto.AssignedToId,dto.RoomId,dto.ScheduledAt, _currentTenantService.TenantId, dto.PropertyId);
        var result = await _repository.Add(entity);
        return result;
    }

    public async Task UpdateTask(UpdateHousekeepingTaskDto dto)
    {
        HouseKeepingTask task = await _repository.GetById(dto.Id);
        task.UpdateHouseKeepingTask(dto.Title,dto.AssignedToId);
        await _repository.Update(task);
    }

    public async Task<HouseKeepingTaskDto> GetTaskById(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        List<Guid> ids = new List<Guid>{task.AssignedToId, task.AssignedById};
        var names = await _identityService.GetUserNamesByIds(ids);
        HouseKeepingTaskDto dto = new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, names.GetValueOrDefault(task.AssignedToId,"Unknown"), task.AssignedById, names.GetValueOrDefault(task.AssignedById,"Unknown"),
            task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt);
        return dto;
    }

    public async Task<PaginatedResult<HouseKeepingTaskDto>> GetAllTasks(int currentPage, int pageSize, HouseTaskFilterDto filters)
    {
        Expression<Func<HouseKeepingTask, bool>> predicate = t =>
            (!filters.Status.HasValue || t.Status == filters.Status) &&
            (!filters.ScheduledFrom.HasValue || t.ScheduledAt >= filters.ScheduledFrom) &&
            (!filters.ScheduledTo.HasValue || t.ScheduledAt <= filters.ScheduledTo);
        (List<HouseKeepingTask> response, int totalCount) = await _repository.GetByConditionPaginated(predicate,currentPage,pageSize);
        var idsTo = response.Select(task => task.AssignedToId).ToList();
        var idsBy = response.Select(task => task.AssignedById).ToList();
        var ids = idsTo.Concat(idsBy);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<HouseKeepingTaskDto> list = response.Select(task => new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, names.GetValueOrDefault(task.AssignedToId, "Unknown"), task.AssignedById,names.GetValueOrDefault(task.AssignedById, "Unknown") , task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt)).ToList();
        return new PaginatedResult<HouseKeepingTaskDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<PaginatedResult<HouseKeepingTaskDto>> GetTaskByAssignedUser(Guid id, int currentPage, int pageSize,HouseTaskFilterDto filters)
    {
        Expression<Func<HouseKeepingTask, bool>> predicate = t =>
            t.AssignedToId == id &&
            (!filters.Status.HasValue || t.Status == filters.Status) &&
            (!filters.ScheduledFrom.HasValue || t.ScheduledAt >= filters.ScheduledFrom) &&
            (!filters.ScheduledTo.HasValue || t.ScheduledAt <= filters.ScheduledTo);
        
        (List<HouseKeepingTask> response,int totalCount) = await _repository.GetByConditionPaginated(predicate, currentPage, pageSize);
        var idsTo = response.Select(task => task.AssignedToId).ToList();
        var idsBy = response.Select(task => task.AssignedById).ToList();
        var ids = idsTo.Concat(idsBy);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<HouseKeepingTaskDto> list = response.Select(task => new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, names.GetValueOrDefault(task.AssignedToId, "Unknown"),task.AssignedById, names.GetValueOrDefault(task.AssignedById, "Unknown"), task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt)).ToList();
        return new PaginatedResult<HouseKeepingTaskDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<PaginatedResult<HouseKeepingTaskDto>> GetTasksByProperty(Guid id,int currentPage, int pageSize, HouseTaskFilterDto filters)
    {  Expression<Func<HouseKeepingTask, bool>> predicate = t =>
            t.PropertyId ==id &&
            (!filters.Status.HasValue || t.Status == filters.Status) &&
            (!filters.ScheduledFrom.HasValue || t.ScheduledAt >= filters.ScheduledFrom) &&
            (!filters.ScheduledTo.HasValue || t.ScheduledAt <= filters.ScheduledTo);
        (List<HouseKeepingTask> response, int totalCount) = await _repository.GetByConditionPaginated(predicate, currentPage, pageSize);
        var idsTo = response.Select(task => task.AssignedToId).ToList();
        var idsBy = response.Select(task => task.AssignedById).ToList();
        var ids = idsTo.Concat(idsBy);
        var names = await _identityService.GetUserNamesByIds(ids);
        List<HouseKeepingTaskDto> list = response.Select(task => new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, names.GetValueOrDefault(task.AssignedToId, "Unknown"), task.AssignedById,names.GetValueOrDefault(task.AssignedById, "Unknown"), task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt)).ToList();
        return new PaginatedResult<HouseKeepingTaskDto>(list,totalCount,pageSize,currentPage);
    }

    public async  Task<bool> DeleteTask(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }

    public async Task StartTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        var room = await _roomRepository.GetById(task.RoomId);
        task.Start();
        if (room.Status != RoomStatus.Cleaning)
        {
            room.SetCleaning();
            await _roomRepository.Update(room);
        }
        await _repository.Update(task);
        

    }

    public async Task CompleteTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        var room = await _roomRepository.GetById(task.RoomId);
        task.Complete();
        room.SetAvailable();
        await _roomRepository.Update(room);
        await _repository.Update(task);
    }

    public async Task CancelTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        if (task.Status == HouseKeepingTaskStatus.InProgress)
        {
            Room room = await _roomRepository.GetById(task.RoomId);
            room.SetAvailable();
            await _roomRepository.Update(room);
        }
        task.Cancel();
        await _repository.Update(task);
    }

    public async Task ReassignTask(Guid id, Guid newStaffId)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        task.Reassign(newStaffId);
        await _repository.Update(task);
    }
}