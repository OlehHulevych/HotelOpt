using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace HotelOpt.Hubs;

public class NotificationSender:INotificationSender
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationSender(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendToTenantAsync(Guid tenantId, string alertType, string message)
    {
        await _hubContext.Clients.Group($"tenant-{tenantId}").SendAsync(alertType,message);
    }
}