using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class StaffFairnessService:IStaffFairnessService
{
    private readonly IRepository<HouseKeepingTask> _repository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IIdentityService _identityService;

    public StaffFairnessService(IRepository<HouseKeepingTask> repository, ICurrentTenantService currentTenantService, IIdentityService identityService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _identityService = identityService;
    }
    public async Task<List<StaffFairnessDto>> GetStaffFairness()
    {
        DateTimeOffset today = DateTimeOffset.UtcNow;
        var daysFromMonday = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-daysFromMonday) - today.TimeOfDay;
        var query = await _repository.GetByCondition(t=> t.TenantId == _currentTenantService.TenantId && t.ScheduledAt >= weekStart && t.Status != HouseKeepingTaskStatus.Cancelled);
        var ids = query.Select(t => t.AssignedToId).Distinct().ToList();
        var names = await _identityService.GetUserNamesByIds(ids);
        return query.GroupBy(t=>t.AssignedToId).Select(t=>new StaffFairnessDto(t.Key,names.GetValueOrDefault(t.Key,"Unknown"),t.Count())).ToList();
        
    }
}