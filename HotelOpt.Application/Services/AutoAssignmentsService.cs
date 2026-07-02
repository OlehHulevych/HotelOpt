using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class AutoAssignmentsService:IAutoAssignmentService
{
    private readonly IRepository<HouseKeepingTask> _taskRepository;
    private readonly IRepository<Shift> _shiftRepository;

    public AutoAssignmentsService(IRepository<HouseKeepingTask> taskRepository, IRepository<Shift> shiftRepository)
    {
        _shiftRepository = shiftRepository;
        _taskRepository = taskRepository;
    }
    public async Task AssignDailyHousekeepingTasks()
    {
        var tommorow = new DateTimeOffset(DateTimeOffset.UtcNow.Date.AddDays(1), TimeSpan.Zero);;
        var dayAfter = tommorow.AddDays(1);
        var unassignedTasks = await _taskRepository.GetByCondition(t=> t.ScheduledAt >= tommorow
                                                                  && t.ScheduledAt<= dayAfter && t.Status == HouseKeepingTaskStatus.Pending);
        if (unassignedTasks.Count < 1) return;
        var shifts = await _shiftRepository.GetByCondition(s=>s.StartTime>=tommorow && s.EndTime<=dayAfter && s.Status == ShiftStatus.Scheduled );
        List<Guid> staffIds = shifts.Select(s => s.StaffId).Distinct().ToList();
        if (staffIds.Count < 1) return;
        int staffIndex = 0;
        var DaysFromMonday = ((int)DateTimeOffset.UtcNow.DayOfWeek + 6) % 7;
        var weekStart = DateTimeOffset.UtcNow.AddDays(-DaysFromMonday);
        var weeklyTasks = await _taskRepository.GetByCondition(t =>
            t.ScheduledAt >= weekStart && t.Status != HouseKeepingTaskStatus.Cancelled);
        Dictionary<Guid, int> weeklyCounts = weeklyTasks.GroupBy(t=>t.AssignedToId).ToDictionary(g=>g.Key, g=>g.Count());
        staffIds = staffIds.Where(id => !weeklyCounts.ContainsKey(id) || weeklyCounts[id] < 25).ToList();
        if (staffIds.Count < 1) return;
        for (int i = 0; i < unassignedTasks.Count; i++)
        {
            unassignedTasks[i].Reassign(staffIds[staffIndex]);
            staffIndex = (staffIndex + 1) % staffIds.Count;
            await _taskRepository.Update(unassignedTasks[i]);
        }

        
        
    }
}