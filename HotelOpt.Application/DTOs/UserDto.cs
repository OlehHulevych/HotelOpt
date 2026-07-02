using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UserDto(string FirstName, string SecondName, string Email, Guid Id, Guid TenantId, UserRole Role );