using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UserDto(string FirstName, string SecondName, string Email, string? AvatarUrl, Guid Id, Guid TenantId, UserRole Role,Guid? PropertyId =null );