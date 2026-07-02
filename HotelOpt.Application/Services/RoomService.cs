using FluentValidation;
using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Services;

public class RoomService:IRoomService
{
    private readonly IRepository<Room> _repository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IValidator<CreateRoomDto> _createValidator;
    private readonly IValidator<UpdateRoomDto> _updateValidator;

    public RoomService (IRepository<Room> repository, ICurrentTenantService currentTenantService, IValidator<CreateRoomDto> createValidator, IValidator<UpdateRoomDto> updateValidator)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }
    
    public async Task<bool> AddRoom(CreateRoomDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        Room newRoom = new Room(dto.RoomNumber,dto.Description,dto.PropertyId,dto.Type, _currentTenantService.TenantId);
        var result = await _repository.Add(newRoom);
        return result;
    }

    public async Task UpdateRoom(UpdateRoomDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        Room room = await _repository.GetById(dto.Id);
        room.Update(dto.RoomNumber,dto.Description,dto.PropertyId,dto.Type,dto.Status);
        await _repository.Update(room);
    }

    public async Task<PaginatedResult<RoomDto>> GetAllRooms(int pageSize, int currentPage)
    {
        var (response, totalCount) = await _repository.GetAllPaginated(currentPage, pageSize);
        List<RoomDto> list = response.Select(r=>new RoomDto(r.Id,r.RoomNumber,r.Description,r.Status,r.Type,r.PropertyId,r.TenantId)).ToList();
        return new PaginatedResult<RoomDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<PaginatedResult<RoomDto>> GetAllRoomsByProperty(Guid propertyId, int pageSize, int currentPage)
    {
        var (response, totalCount) = await _repository.GetByConditionPaginated((r=>r.PropertyId==propertyId),currentPage, pageSize);
        List<RoomDto> list = response.Select(r=>new RoomDto(r.Id,r.RoomNumber,r.Description,r.Status,r.Type,r.PropertyId,r.TenantId)).ToList();
        return new PaginatedResult<RoomDto>(list,totalCount,pageSize,currentPage);
    }

    public async Task<RoomDto> GetRoomById(Guid id)
    {
        Room entity = await _repository.GetById(id);
        RoomDto room = new RoomDto(entity.Id,entity.RoomNumber,entity.Description,entity.Status,entity.Type,entity.PropertyId,entity.TenantId);
        return room;
    }

    public async Task<bool> DeleteRoom(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }
}