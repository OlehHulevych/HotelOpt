using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record UpdateRoomDto(Guid Id, string? RoomNumber, string? Description, Guid? PropertyId, RoomType? Type, RoomStatus? Status);