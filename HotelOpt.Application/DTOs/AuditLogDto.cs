using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record AuditLogDto(Guid Id, string EntityName, Guid EntityId, EntityAction Action, Guid ChangedById, string Changes, DateTimeOffset CreatedAt);
