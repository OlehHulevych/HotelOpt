namespace HotelOpt.Application.DTOs;

public record PropertyDto(Guid Id, string Name, string ContactEmail, string PhoneNumber, string Address,int StarRating, Guid TenantId);