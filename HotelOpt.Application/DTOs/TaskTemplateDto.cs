using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record TaskTemplateItemDto(Guid Id, string Title, int Order);
public record TaskTemplateDto(Guid Id, string Name, RoomType RoomType, Guid PropertyId, List<TaskTemplateItemDto> Items);
