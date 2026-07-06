using HotelOpt.Application.DTOs;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> CreateAsync(CreateBookingDto dto);
    Task CheckInAsync(Guid bookingId);
    Task CheckOutAsync(Guid bookingId);
    Task CancelAsync(Guid bookingId);
    Task AddGuestAsync(Guid bookingId, Guid guestId);
    Task<PaginatedResult<BookingResponseDto>> GetByPropertyAsync(Guid propertyId, int page, int pageSize);
}
