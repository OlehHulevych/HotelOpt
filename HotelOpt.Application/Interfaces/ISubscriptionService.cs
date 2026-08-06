using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Interfaces;

public interface ISubscriptionService
{
    Task SubscribeAsync(Guid tenantId, SubscriptionPlan plan, string priceId);
    Task CancelAsync();
    Task HandleWebhookAsync(string json, string stripeSignature, string webHookSecret);
    Task<TenantSubscriptionDto> GetStatusAsync();
    
}
