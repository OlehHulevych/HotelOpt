using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class SmartAlertService:ISmartAlertService
{
    private readonly IRepository<HouseKeepingTask> _houseKeepingTaskRepository;
    private readonly IRepository<MaintenanceTicket> _maintenanceTicketRepository;
    private readonly IRepository<Shift> _shiftRepository;
    private readonly INotificationSender _notificationSender;

    public SmartAlertService(IRepository<HouseKeepingTask> houseKeepingTaskRepository,
        IRepository<MaintenanceTicket> maintenanceTicketRepository, IRepository<Shift> shiftRepository,
        INotificationSender notificationSender)
    {
        _houseKeepingTaskRepository = houseKeepingTaskRepository;
        _maintenanceTicketRepository = maintenanceTicketRepository;
        _shiftRepository = shiftRepository;
        _notificationSender = notificationSender;
    }
    public async Task CheckAndSendAlerts()
    {
        var overdueTasks = await _houseKeepingTaskRepository.GetByCondition(task => task.ScheduledAt < DateTimeOffset.UtcNow && task.Status == HouseKeepingTaskStatus.Pending);
        var overdueTickets = await _maintenanceTicketRepository.GetByCondition(ticket =>
            ticket.CreatedAt < DateTimeOffset.UtcNow.AddHours(-24) && ticket.Status == TicketStatus.Open);
        var tommorow = new DateTimeOffset(DateTimeOffset.UtcNow.Date.AddDays(1), TimeSpan.Zero);
        var dayAfter = tommorow.AddDays(1);
        var unStaffedShifts = await _shiftRepository.GetByCondition(shift=>shift.StartTime>=tommorow && shift.StartTime<dayAfter && shift.Status == ShiftStatus.Scheduled);

        var groupedTasks = overdueTasks.GroupBy(t => new { t.TenantId, t.PropertyId });
        foreach (var group in groupedTasks)
        {
            await _notificationSender.SendToTenantAsync(group.Key.TenantId, "OverdueTask",
                $"Property {group.Key.PropertyId} has only {group.Count()} overdue tasks");
        }
        var groupedTickets = overdueTickets.GroupBy(t => new { t.TenantId, t.PropertyId });
        foreach (var group in groupedTickets)
        {
            await _notificationSender.SendToTenantAsync(group.Key.TenantId, "OverdueTicket",
                $"Property {group.Key.PropertyId} has only {group.Count()} tickets overdue");
        }
        var groupedShifts = unStaffedShifts.GroupBy(s => new { s.TenantId, s.PropertyId });
        foreach (var group in groupedShifts)
        {
            if (group.Count() < 2)
            {
                await _notificationSender.SendToTenantAsync(
                    group.Key.TenantId,
                    "UnderstaffedShift",
                    $"Property {group.Key.PropertyId} has only {group.Count()} staff scheduled for tomorrow");
            }
        }

    }
}