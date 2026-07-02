using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Services;

public class ShiftService:IShiftService
{
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IRepository<Shift> _repository;
    private readonly IIdentityService _identityService;

    public ShiftService(ICurrentTenantService currentTenantService, IRepository<Shift> repository, IIdentityService identityService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _identityService = identityService;
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

    public async Task<PaginatedResult<ShiftDto>> GetAllShifts(int currentPage, int pageSize)
    {
        (List<Shift> shifts, int totalCount) = await _repository.GetAllPaginated( currentPage,  pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await _identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<PaginatedResult<ShiftDto>> GetAllShiftsByProperty(Guid id, int currentPage, int pageSize)
    {
        (List<Shift> shifts, int totalCount) = await _repository.GetByConditionPaginated(e=>e.PropertyId == id, currentPage, pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await _identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId,"Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<ShiftDto> GetShiftById(Guid id)
    {
        Shift shift = await _repository.GetById(id);
        var ids = new List<Guid> { shift.StaffId};
        var names = await _identityService.GetUserNamesByIds(ids);
        ShiftDto shiftDto = new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId,shift.PropertyId,shift.StaffId,names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status);
        return shiftDto;
    }

    public async Task<PaginatedResult<ShiftDto>> GetShiftByStaff(Guid id, int currentPage, int pageSize)
    {
        (List<Shift> shifts, int totalCount) = await _repository.GetByConditionPaginated(e=>e.StaffId == id, currentPage, pageSize);
        var ids =  shifts.Select(s => s.StaffId).ToList();
        var names = await _identityService.GetUserNamesByIds(ids);
        List<ShiftDto> list = shifts.Select(shift => new ShiftDto(shift.Id, shift.StartTime, shift.EndTime,shift.TenantId, shift.PropertyId,shift.StaffId, names.GetValueOrDefault(shift.StaffId, "Unknown"), shift.Status)).ToList();
        return new PaginatedResult<ShiftDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<bool> DeleteShift(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }
}