namespace HotelOpt.Application.DTOs;

public record UpdateGuestDto(Guid Id, string? FirstName, string? LastName, string? Email, string? Phone);
