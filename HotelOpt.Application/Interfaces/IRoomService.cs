using HotelOpt.Application.DTOs;
using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IRoomService
{
    public Task<bool> AddRoom(CreateRoomDto dto);
    public Task UpdateRoom(UpdateRoomDto dto);
    public Task<List<RoomDto>> GetAllRooms();
    public Task<RoomDto> GetRoomById(Guid id);
    public Task<bool> DeleteRoom(Guid id);
}