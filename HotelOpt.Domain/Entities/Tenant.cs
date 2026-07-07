using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;


public class Tenant:BaseEntity
{
    public string Name { get; private set; } 
    public string ContactEmail { get; private set; }
    public TenantStatus Status { get; private set; } = TenantStatus.Pending;
    public string? StripeCustomerId { get; private set; }
    public string? StripeSubscriptionId { get;private set; }
    public SubscriptionPlan SubscriptionPlan { get; private set; }
    public SubscriptionStatus SubscriptionStatus { get; private set; }
    public DateTimeOffset TrialEndsAt { get; private set; }

    private Tenant()
    {
        Name = null!;
        ContactEmail = null!;
        SubscriptionPlan = SubscriptionPlan.Trial;
        SubscriptionStatus = SubscriptionStatus.Active;
        TrialEndsAt = DateTimeOffset.MinValue;
    }

    public  Tenant(string name, string contactEmail)
    {
        Name = name;
        ContactEmail = contactEmail;
        StartTrial();
    }

    public void StartTrial()
    {
        if (SubscriptionPlan == SubscriptionPlan.Pro || SubscriptionPlan == SubscriptionPlan.Basic)
            throw new Exception($"You can't use trial version because you have already use {SubscriptionPlan.ToString()}");
        SubscriptionPlan = SubscriptionPlan.Trial;
        SubscriptionStatus = SubscriptionStatus.Active;
        TrialEndsAt = DateTimeOffset.UtcNow.AddDays(30);
    }
    

}