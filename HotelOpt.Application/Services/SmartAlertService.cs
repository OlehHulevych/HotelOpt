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
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly IRepository<Tenant> _tenantRepository;

    public SmartAlertService(IRepository<HouseKeepingTask> houseKeepingTaskRepository,
        IRepository<MaintenanceTicket> maintenanceTicketRepository, IRepository<Shift> shiftRepository,
        INotificationSender notificationSender, IIdentityService identityService, IEmailService emailService, IRepository<Tenant> tenantRepository)
    {
        _houseKeepingTaskRepository = houseKeepingTaskRepository;
        _maintenanceTicketRepository = maintenanceTicketRepository;
        _shiftRepository = shiftRepository;
        _notificationSender = notificationSender;
        _identityService = identityService;
        _emailService = emailService;
        _tenantRepository = tenantRepository;
    }
    public async Task CheckAndSendAlerts()
    {
        var overdueTasks = await _houseKeepingTaskRepository.GetByCondition(task => task.ScheduledAt < DateTimeOffset.UtcNow && task.Status == HouseKeepingTaskStatus.Pending);
        var overdueTickets = await _maintenanceTicketRepository.GetByCondition(ticket =>
            ticket.CreatedAt < DateTimeOffset.UtcNow.AddHours(-24) && ticket.Status == TicketStatus.Open);
        var tommorow = new DateTimeOffset(DateTimeOffset.UtcNow.Date.AddDays(1), TimeSpan.Zero);
        var dayAfter = tommorow.AddDays(1);
        var unStaffedShifts = await _shiftRepository.GetByCondition(shift=>shift.StartTime>=tommorow && shift.StartTime<dayAfter && shift.Status == ShiftStatus.Scheduled);
        foreach (var task in overdueTasks)
        {
            var email = await _identityService.GetUserEmailAsync(task.AssignedToId);
            if (email != null)
                await _emailService.SendAsync(email, "Overdue Housekeeping task",
                    $"Your housekeeping task scheduled for {task.ScheduledAt} is overdue. Please complete it as soon as possible.");
        }    
        var groupedTasks = overdueTasks.GroupBy(t => new { t.TenantId, t.PropertyId });
        foreach (var group in groupedTasks)
        {
            await _notificationSender.SendToTenantAsync(group.Key.TenantId, "OverdueTask",
                $"Property {group.Key.PropertyId} has only {group.Count()} overdue tasks");
        }
        foreach (var ticket in overdueTickets)
        {
            var email = await _identityService.GetUserEmailAsync(ticket.StaffId);
            if (email != null)
                await _emailService.SendAsync(email, "Overdue Maintenance Ticket",
                    $"Maintenance ticket '{ticket.Title}' created on {ticket.CreatedAt} is overdue. Please resolve it.");
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
                var tenant = await _tenantRepository.GetById(group.Key.TenantId);
                await _emailService.SendAsync(tenant.ContactEmail, "Understaffed Shift Alert ", $"Property {group.Key.PropertyId} has only {group.Count()} staff scheduled for tomorrow. Please arrange additional coverage.");
            }
        }

    }
}