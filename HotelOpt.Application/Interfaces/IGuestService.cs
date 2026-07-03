using HotelOpt.Application.DTOs;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IGuestService
{
    Task<bool> AddGuest(CreateGuestDto dto);
    Task UpdateGuest(UpdateGuestDto dto);
    Task<GuestDto> GetById(Guid id);
    Task<PaginatedResult<GuestDto>> GetAll(int currentPage, int pageSize);
    Task<bool> DeleteGuest(Guid id);
}
