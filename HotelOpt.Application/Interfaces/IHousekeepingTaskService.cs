using HotelOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IHousekeepingTaskService
{
    public Task<bool> CreateTask(CreateHousekeepingTaskDto dto);
    public Task UpdateTask(UpdateHousekeepingTaskDto dto);
    public Task<HouseKeepingTaskDto> GetTaskById(Guid id);
    public Task<List<HouseKeepingTaskDto>> GetAllTasks();
    public Task<List<HouseKeepingTaskDto>> GetTaskByAssignedUser(Guid id);
    public Task<bool> DeleteTask(Guid id);
    public Task StartTask(Guid id);
    public Task CompleteTask(Guid id);
    public Task CancelTask(Guid id);
    public Task ReassignTask(Guid id, Guid newStaffId);

}