using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Interfaces;

public interface ISubscriptionService
{
    Task SubscribeAsync(Guid tenantId, SubscriptionPlan plan, string priceId);
    Task CancelAsync(Guid tenantId);
    Task HandleWebhookAsync(string json, string stripeSignature, string webHookSecret);
}
