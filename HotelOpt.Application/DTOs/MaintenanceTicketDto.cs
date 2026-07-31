using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record MaintenanceTicketDto(Guid Id, string Title, string Description, Guid StaffId, string StaffName, Guid ReportedId, string ReportedName, Guid RoomId, string RoomNumber, Guid PropertyId, TicketPriority Priority, TicketStatus Status, DateTimeOffset? ResolvedAt);