using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record CreateTaskTemplateItemDto(string Title, int Order);
public record CreateTaskTemplateDto(string Name, RoomType RoomType, Guid PropertyId, List<CreateTaskTemplateItemDto> Items);
