namespace HotelOpt.Application.Interfaces;

public interface IStripeService
{
    public Task<string> CreateCustomerAsync(string email, string tenantName);
    public Task<string> CreatSubscriptionAsync(string customerId, string priceId);
    public Task CancelSubscriptionAsync(string subscriptionId);
}