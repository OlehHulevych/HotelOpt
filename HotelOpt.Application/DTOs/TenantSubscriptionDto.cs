namespace HotelOpt.Application.DTOs;

public record TenantSubscriptionDto(string Plan, string Status, DateTimeOffset TrialEndAt);