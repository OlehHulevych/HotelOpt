using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record CreateRoomDto(string RoomNumber, string Description, Guid PropertyId, RoomType Type);