using Microsoft.AspNetCore.SignalR;

namespace HotelOpt.Hubs;

public class NotificationHub:Hub
{
    public async override Task OnConnectedAsync()
    {
        var tenantId = Context.User.FindFirst("TenantId")?.Value;
        await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant-{tenantId}");
        await base.OnConnectedAsync();
    }
}