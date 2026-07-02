using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Infrastructure.Services;

public class RoomInspectionService:IRoomInspectionService
{
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IRepository<RoomInspection> _repository;

    public RoomInspectionService( ICurrentTenantService currentTenantService, IRepository<RoomInspection> repository)
    {
        _currentTenantService = currentTenantService;
        _repository = repository;
    }
    
    public async Task<RoomInspection> CreateInspectionAsync(Guid roomId, Guid propertyId, Guid inspectedById, string photoUrl,
        GeminiInspectionResult result)
    {
        RoomInspection inspection = new RoomInspection(roomId, _currentTenantService.TenantId,inspectedById, propertyId,result.Issues,photoUrl,result.Passed);
        await _repository.Add(inspection);
        return inspection;

    }

    public async Task<PaginatedResult<RoomInspection>> GetInspectionsByRoomAsync(Guid roomId, int page, int pageSize)
    {
        (List<RoomInspection> list, int total) = await _repository.GetByConditionPaginated(i=>i.RoomId==roomId, page, pageSize);
        PaginatedResult<RoomInspection> result = new PaginatedResult<RoomInspection>(list, total, pageSize, page);
        return result;
    }
}