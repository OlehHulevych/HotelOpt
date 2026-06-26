namespace HotelOpt.Application.DTOs;

public record MessageDto(Guid Id, string Content, Guid SenderId, Guid PropertyId, DateTimeOffset CreatedAt);