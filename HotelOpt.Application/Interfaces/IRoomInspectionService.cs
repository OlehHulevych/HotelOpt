using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IRoomInspectionService
{
    Task<RoomInspection> CreateInspectionAsync(Guid roomId, Guid propertyId, Guid inspectedById, string photoUrl, GeminiInspectionResult result);
    Task<PaginatedResult<RoomInspection>> GetInspectionsByRoomAsync(Guid roomId, int page, int pageSize);}