using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record HouseKeepingTaskDto( Guid Id, string Title, Guid AssignedToId, Guid AssignedById, Guid RoomId, HouseKeepingTaskStatus Status, DateTimeOffset ScheduledAt, DateTimeOffset? CompletedAt);