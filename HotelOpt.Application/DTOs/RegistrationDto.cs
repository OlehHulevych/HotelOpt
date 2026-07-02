using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record RegistrationDto(string Name, string Surname, string Email, string Password, UserRole Role, Guid TenantId);