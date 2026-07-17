using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IAuditLogService
{
    Task<List<AuditLogDto>> GetByEntityAsync(string entityName, Guid entityId);
    Task<List<AuditLogDto>> GetAllAsync();
}
