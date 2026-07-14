using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using Microsoft.Extensions.Options;

namespace HotelOpt.Application.Services;

public class SubscriptionService:ISubscriptionService
{
    private readonly IStripeService _stripeService;
    private readonly IRepository<Tenant> _repository;
    private readonly ICurrentTenantService _currentTenantService;

     
    

    public SubscriptionService(IStripeService stripeService, IRepository<Tenant> repository, ICurrentTenantService currentTenantService)
    {
        _stripeService = stripeService;
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task SubscribeAsync(Guid tenantId, SubscriptionPlan plan, string priceId)
    {
        Tenant tenant = await _repository.GetById(tenantId);
        string customerId = await _stripeService.CreateCustomerAsync(tenant.ContactEmail, tenant.Name);
        string subscriptionId = await _stripeService.CreatSubscriptionAsync(customerId, priceId);
        tenant.SetSubscription(customerId, subscriptionId, plan);
        await _repository.Update(tenant);
    }

    public async Task CancelAsync(Guid tenantId)
    {
        Tenant tenant = await _repository.GetById(tenantId);
        if (tenant.StripeSubscriptionId == null) throw new Exception("No active subscription");
        await _stripeService.CancelSubscriptionAsync(tenant.StripeSubscriptionId);
        tenant.UpdateSubscriptionStatus(SubscriptionStatus.Cancelled);
        await _repository.Update(tenant);
    }

    public async Task HandleWebhookAsync(string json, string stripeSignature,string webhookSecret)
    {
        var result = _stripeService.ParseWebhookEvent(json, stripeSignature, webhookSecret);
        var tenants = await _repository.GetByCondition(t => t.StripeCustomerId == result.CustomerId);
        var tenant = tenants.FirstOrDefault();
        if (tenant == null) return;

        switch (result.EventType)
        {
            case "invoice.paid":
                tenant.UpdateSubscriptionStatus(SubscriptionStatus.Active);
                break;
            case "invoice.payment_failed":
                tenant.UpdateSubscriptionStatus(SubscriptionStatus.PastDue);
                break;
            case "customer.subscription.deleted":
                tenant.UpdateSubscriptionStatus(SubscriptionStatus.Locked);
                break;
        }

        await _repository.Update(tenant);
    }
}