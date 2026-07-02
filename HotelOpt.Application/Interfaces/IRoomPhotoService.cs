using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Interfaces;

public interface IRoomPhotoService
{
    Task<RoomPhoto> UploadPhotoAsync(Guid roomId, Guid uploadedById, string url);                                                                                                                                                     
    Task<PaginatedResult<RoomPhoto>> GetPhotosByRoomAsync(Guid roomId, int page, int pageSize);
}