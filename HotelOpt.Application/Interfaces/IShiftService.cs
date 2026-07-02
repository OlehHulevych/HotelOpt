using HotelOpt.Application.DTOs;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IShiftService
{
    public Task<bool> AddShift(CreateShiftDto dto);
    public Task UpdateShift(UpdateShiftDto dto);
    public Task<PaginatedResult<ShiftDto>> GetAllShifts(int currentPage, int pageSize);
    public Task<PaginatedResult<ShiftDto>> GetAllShiftsByProperty(Guid id, int currentPage, int pageSize);
    public Task<ShiftDto> GetShiftById(Guid id);
    public Task<PaginatedResult<ShiftDto>> GetShiftByStaff(Guid id, int currentPage, int pageSize);
    public Task<bool> DeleteShift(Guid id);
}