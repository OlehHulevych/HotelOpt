using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record CreateHousekeepingTaskDto( string Title, Guid AssignedToId, Guid RoomId, DateTimeOffset ScheduledAt, Guid PropertyId );