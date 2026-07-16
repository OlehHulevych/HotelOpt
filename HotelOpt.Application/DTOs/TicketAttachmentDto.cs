namespace HotelOpt.Application.DTOs;

public record TicketAttachmentDto(Guid Id, Guid TicketId, string FileName, string BlobUrl, Guid UploadedById, DateTimeOffset UploadedAt);
