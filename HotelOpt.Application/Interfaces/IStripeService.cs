using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IStripeService
{
    Task<string> CreateCustomerAsync(string email, string tenantName);
    Task<string> CreatSubscriptionAsync(string customerId, string priceId);
    Task CancelSubscriptionAsync(string subscriptionId);
    StripeWebhookResult ParseWebhookEvent(string json, string stripeSignature, string webhookSecret);
}