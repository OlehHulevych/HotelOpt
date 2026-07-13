using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record HouseTaskFilterDto(HouseKeepingTaskStatus? Status, DateTimeOffset? ScheduledFrom, DateTimeOffset? ScheduledTo );