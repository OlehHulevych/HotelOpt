using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record MaintenanceTicketDto(Guid Id, string Title, string Description, Guid StaffId, Guid ReportedId, Guid RoomId, Guid PropertyId, TicketPriority Priority, TicketStatus Status, DateTimeOffset? ResolvedAt);