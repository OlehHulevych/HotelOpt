using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HoteOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

public class RoomService:IRoomService
{
    private readonly IRepository<Room> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public RoomService (IRepository<Room> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    
    public async Task<bool> AddRoom(CreateRoomDto dto)
    {
        Room newRoom = new Room(dto.RoomNumber,dto.Description,dto.PropertyId,dto.Type, _currentTenantService.TenantId);
        var result = await _repository.Add(newRoom);
        return result;
    }

    public async Task UpdateRoom(UpdateRoomDto dto)
    {
        Room room = await _repository.GetById(dto.Id);
        room.Update(dto.RoomNumber,dto.Description,dto.PropertyId,dto.Type,dto.Status);
        await _repository.Update(room);
    }

    public async Task<List<RoomDto>> GetAllRooms()
    {
        var response = await _repository.GetAll();
        List<RoomDto> list = response.Select(r=>new RoomDto(r.Id,r.RoomNumber,r.Description,r.Status,r.Type,r.PropertyId,r.TenantId)).ToList();
        return list;
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