using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HoteOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class ShiftService:IShiftService
{
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IRepository<Shift> _repository;

    public ShiftService(ICurrentTenantService currentTenantService, IRepository<Shift> repository)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task<bool> AddShift(CreateShiftDto dto)
    {
        Shift shift = new Shift(dto.StaffId, dto.PropertyId,_currentTenantService.TenantId, dto.StartTime, dto.EndTime);
        var result = await _repository.Add(shift);
        return result;
    }

    public async Task UpdateShift(UpdateShiftDto dto)
    {
        Shift shift = await _repository.GetById(dto.Id);
        shift.Update(dto.StartTime,dto.EndTime,dto.StaffId,dto.Status);
        await _repository.Update(shift);
    }

    public async Task<List<ShiftDto>> GetAllShifts()
    {
        List<Shift> shifts = await _repository.GetAll();
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, shift.Status)).ToList();
        return list;
    }

    public async Task<List<ShiftDto>> GetAllShiftsByProperty(Guid id)
    {
        List<Shift> shifts = await _repository.GetByCondition(e=>e.PropertyId == id);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, shift.Status)).ToList();
        return list;
    }

    public async Task<ShiftDto> GetShiftById(Guid id)
    {
        Shift shift = await _repository.GetById(id);
        ShiftDto shiftDto = new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId,shift.PropertyId,shift.StaffId,shift.Status);
        return shiftDto;
    }

    public async Task<List<ShiftDto>> GetShiftByStaff(Guid id)
    {
        List<Shift> shifts = await _repository.GetByCondition(e=>e.StaffId == id);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, shift.Status)).ToList();
        return list;
    }

    public async Task<bool> DeleteShift(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }
}