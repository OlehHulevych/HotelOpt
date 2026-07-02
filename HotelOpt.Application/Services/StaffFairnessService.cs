using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class StaffFairnessService:IStaffFairnessService
{
    private readonly IRepository<HouseKeepingTask> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public StaffFairnessService(IRepository<HouseKeepingTask> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task<List<StaffFairnessDto>> GetStaffFairness()
    {
        DateTimeOffset today = DateTimeOffset.UtcNow;
        var daysFromMonday = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-daysFromMonday);
        var query = await _repository.GetByCondition(t=> t.TenantId == _currentTenantService.TenantId && t.ScheduledAt >= weekStart && t.Status != HouseKeepingTaskStatus.Cancelled);
        return query.GroupBy(t=>t.AssignedToId).Select(t=>new StaffFairnessDto(t.Key,t.Count())).ToList();
        
    }
}