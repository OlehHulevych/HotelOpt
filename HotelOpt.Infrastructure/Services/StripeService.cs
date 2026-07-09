using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Stripe;

namespace HotelOpt.Infrastructure.Services;

public class StripeService:IStripeService
{
    public async Task<string> CreateCustomerAsync(string email, string tenantName)
    {
        var options = new CustomerCreateOptions
        {
            Name = tenantName,
            Email = email
        };
        var service = new CustomerService();
        Customer customer = await service.CreateAsync(options);
        return customer.Id;
    }

    public async Task<string> CreatSubscriptionAsync(string customerId, string priceId)
    {
        var options = new SubscriptionCreateOptions()
        {
            Customer = customerId,
            Items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions { Price = priceId }
            },
        };
        var service = new SubscriptionService();
        Subscription subscription = await service.CreateAsync(options);
        return subscription.Id;
    }

    public async Task CancelSubscriptionAsync(string subscriptionId)
    {
        var service = new SubscriptionService();
        await service.CancelAsync(subscriptionId);
    }

    public StripeWebhookResult ParseWebhookEvent(string json, string stripeSignature, string webhookSecret)
    {
        var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret, throwOnApiVersionMismatch: false);
        var customerId = stripeEvent.Data.Object is Invoice invoice ? invoice.CustomerId : string.Empty;
        return new StripeWebhookResult(stripeEvent.Type, customerId);
    }
}