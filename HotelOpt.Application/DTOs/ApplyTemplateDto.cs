namespace HotelOpt.Application.DTOs;

public record ApplyTemplateDto(Guid RoomId, Guid AssignedToId, DateTimeOffset ScheduledAt);
