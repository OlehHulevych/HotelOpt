using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IRepository<AuditLog> _repository;

    public AuditLogService(IRepository<AuditLog> repository)
    {
        _repository = repository;
    }

    public async Task<List<AuditLogDto>> GetByEntityAsync(string entityName, Guid entityId)
    {
        var logs = await _repository.GetByCondition(a => a.EntityName == entityName && a.EntityId == entityId);
        return logs.Select(a => new AuditLogDto(a.Id, a.EntityName, a.EntityId, a.AuditAction, a.ChangedById, a.Changes, a.CreatedAt)).ToList();
    }

    public async Task<List<AuditLogDto>> GetAllAsync()
    {
        var logs = await _repository.GetAll();
        return logs.Select(a => new AuditLogDto(a.Id, a.EntityName, a.EntityId, a.AuditAction, a.ChangedById, a.Changes, a.CreatedAt)).ToList();
    }
}
