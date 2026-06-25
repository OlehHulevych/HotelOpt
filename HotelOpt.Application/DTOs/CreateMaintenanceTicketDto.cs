using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record CreateMaintenanceTicketDto(string Title, string Description, Guid StaffId, Guid ReportedId, TicketPriority Priority, Guid RoomId, Guid PropertyId);