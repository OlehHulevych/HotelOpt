namespace HoteOpt.Application.Interfaces;

public interface INotificationSender
{
    Task SendToTenantAsync(Guid tenantId, string alertType, string message);
}