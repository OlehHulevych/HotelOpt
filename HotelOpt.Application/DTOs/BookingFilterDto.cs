using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record BookingFilterDto(BookingStatus? Status, DateTimeOffset? CheckInFrom, DateTimeOffset? CheckInTo, string? SortBy = null, bool SortDescending = false);
