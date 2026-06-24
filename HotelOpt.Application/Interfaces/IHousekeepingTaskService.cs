using HotelOpt.Application.DTOs;
using HoteOpt.Application.Pagination;

namespace HoteOpt.Application.Interfaces;

public interface IHousekeepingTaskService
{
    public Task<bool> CreateTask(CreateHousekeepingTaskDto dto);
    public Task UpdateTask(UpdateHousekeepingTaskDto dto);
    public Task<HouseKeepingTaskDto> GetTaskById(Guid id);
    public Task<PaginatedResult<HouseKeepingTaskDto>> GetAllTasks(int currentPage, int pageSize);
    public Task<PaginatedResult<HouseKeepingTaskDto>> GetTaskByAssignedUser(Guid id, int currentPage, int pageSize);
    Task<PaginatedResult<HouseKeepingTaskDto>> GetTasksByProperty(Guid propertyId, int currentPage, int pageSize);
    public Task<bool> DeleteTask(Guid id);
    public Task StartTask(Guid id);
    public Task CompleteTask(Guid id);
    public Task CancelTask(Guid id);
    public Task ReassignTask(Guid id, Guid newStaffId);

}