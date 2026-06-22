using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HoteOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class HousekeepingTaskService:IHousekeepingTaskService
{
    private readonly IRepository<HouseKeepingTask> _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentTenantService _currentTenantService;

    public HousekeepingTaskService(IRepository<HouseKeepingTask> repository, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _currentTenantService = currentTenantService;
    }
    public async Task<bool> CreateTask(CreateHousekeepingTaskDto dto)
    {
        HouseKeepingTask entity = new HouseKeepingTask(dto.Title, _currentUserService.UserId, dto.AssignedToId,dto.RoomId,dto.ScheduledAt, _currentTenantService.TenantId);
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
        HouseKeepingTaskDto dto = new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, task.AssignedById,
            task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt);
        return dto;
    }

    public async Task<List<HouseKeepingTaskDto>> GetAllTasks()
    {
        List<HouseKeepingTask> list = await _repository.GetAll();
        List<HouseKeepingTaskDto> varList = list.Select(task => new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, task.AssignedById, task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt)).ToList();
        return varList;
    }

    public async Task<List<HouseKeepingTaskDto>> GetTaskByAssignedUser(Guid id)
    {
        List<HouseKeepingTask> list = await _repository.GetByCondition(e=>e.AssignedToId == id);
        List<HouseKeepingTaskDto> varList = list.Select(task => new HouseKeepingTaskDto(task.Id, task.Title, task.AssignedToId, task.AssignedById, task.RoomId, task.Status, task.ScheduledAt, task.CompletedAt)).ToList();
        return varList;
    }

    public async  Task<bool> DeleteTask(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }

    public async Task StartTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        task.Start();
        await _repository.Update(task);

    }

    public async Task CompleteTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
        task.Complete();
        await _repository.Update(task);
    }

    public async Task CancelTask(Guid id)
    {
        HouseKeepingTask task = await _repository.GetById(id);
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