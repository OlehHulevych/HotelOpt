namespace HotelOpt.Application.DTOs;

public record StripeWebhookResult(string EventType, string CustomerId);
