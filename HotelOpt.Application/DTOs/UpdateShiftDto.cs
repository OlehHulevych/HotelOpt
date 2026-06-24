using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UpdateShiftDto(Guid Id, DateTimeOffset? StartTime, DateTimeOffset? EndTime, Guid? StaffId, ShiftStatus? Status);