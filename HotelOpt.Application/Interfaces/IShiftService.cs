using HotelOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IShiftService
{
    public Task<bool> AddShift(CreateShiftDto dto);
    public Task UpdateShift(UpdateShiftDto dto);
    public Task<List<ShiftDto>> GetAllShifts();
    public Task<List<ShiftDto>> GetAllShiftsByProperty(Guid id);
    public Task<ShiftDto> GetShiftById(Guid id);
    public Task<List<ShiftDto>> GetShiftByStaff(Guid id);
    public Task<bool> DeleteShift(Guid id);
}