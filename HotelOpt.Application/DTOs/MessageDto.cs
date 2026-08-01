namespace HotelOpt.Application.DTOs;

public record MessageDto(Guid Id, string Content, Guid SenderId, string SenderName, Guid PropertyId, DateTimeOffset CreatedAt);