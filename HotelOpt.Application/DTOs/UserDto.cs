using HotelOpt.Domain.Enums;

namespace HoteOpt.Application.DTOs;

public record UserDto(string FirstName, string SecondName, string Email, Guid Id, Guid TenantId, UserRole Role );