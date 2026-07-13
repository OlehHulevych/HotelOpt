using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record MaintenanceTicketFilterDto(TicketStatus? Status, TicketPriority? Priority, DateTimeOffset? CreatedFrom, DateTimeOffset? CreatedTo);
