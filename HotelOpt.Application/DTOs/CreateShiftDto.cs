namespace HotelOpt.Application.DTOs;

public record CreateShiftDto(DateTimeOffset StartTime, DateTimeOffset EndTime, Guid PropertyId, Guid StaffId);