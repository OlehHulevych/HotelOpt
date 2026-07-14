namespace HotelOpt.Application.DTOs;

public record ShiftReportDto(Guid StaffId, string StaffName, DateTimeOffset StartTime, DateTimeOffset EndTime, Guid PropertyId, int CompletedTasksCount, List<HouseKeepingTaskDto> CompletedTasks, int ResolvedTicketsCount, List<MaintenanceTicketDto> ResolvedTickets);