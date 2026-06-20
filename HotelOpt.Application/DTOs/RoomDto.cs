using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record RoomDto( Guid Id,string RoomNumber, string Description, RoomStatus Status, RoomType Type, Guid PropertyId, Guid TenantId);