using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UpdateHousekeepingTaskDto(Guid Id,string Title, Guid AssignedToId);