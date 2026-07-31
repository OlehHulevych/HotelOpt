using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record HouseKeepingTaskDto( Guid Id, string Title, Guid AssignedToId, string? AssignedToName, Guid AssignedById,string? AssignedByName, Guid RoomId, string RoomNumber, HouseKeepingTaskStatus Status, DateTimeOffset ScheduledAt, DateTimeOffset? CompletedAt);