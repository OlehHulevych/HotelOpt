using HotelOpt.Application.DTOs;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IRoomService
{
    public Task<bool> AddRoom(CreateRoomDto dto);
    public Task UpdateRoom(UpdateRoomDto dto);
    public Task<PaginatedResult<RoomDto>> GetAllRooms(int pageSize, int currentPage);
    public Task<PaginatedResult<RoomDto>> GetAllRoomsByProperty(Guid propertyId, int pageSize, int currentPage);
    public Task<RoomDto> GetRoomById(Guid id);
    public Task<bool> DeleteRoom(Guid id);
}