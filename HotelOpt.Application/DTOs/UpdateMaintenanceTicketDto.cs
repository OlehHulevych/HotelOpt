using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UpdateMaintenanceTicketDto(Guid Id,string? title, string? description, Guid? staffId, Guid? reportedId, TicketPriority? priority);