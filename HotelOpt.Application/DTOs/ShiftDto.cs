using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record ShiftDto(Guid Id, DateTimeOffset StartTime, DateTimeOffset EndTime, Guid TenantId, Guid PropertyId, Guid StaffId, ShiftStatus Status);