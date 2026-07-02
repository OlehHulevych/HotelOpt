using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class RoomPhotoService:IRoomPhotoService
{
    private readonly IRepository<RoomPhoto> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public RoomPhotoService(IRepository<RoomPhoto> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task<RoomPhoto> UploadPhotoAsync(Guid roomId, Guid uploadedById, string url)
    {
        RoomPhoto newPhoto = new RoomPhoto(roomId, _currentTenantService.TenantId, uploadedById, url);
        await _repository.Add(newPhoto);
        return newPhoto;
    }

    public async Task<PaginatedResult<RoomPhoto>> GetPhotosByRoomAsync(Guid roomId, int page, int pageSize)
    {
        (List<RoomPhoto> list, int total) = await _repository.GetByConditionPaginated((p => p.RoomId == roomId), page, pageSize);
        return new PaginatedResult<RoomPhoto>(list,total,pageSize, page);
    }
}