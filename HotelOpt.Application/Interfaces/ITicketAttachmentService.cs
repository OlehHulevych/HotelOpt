using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface ITicketAttachmentService
{
    Task<TicketAttachmentDto> UploadAsync(Guid ticketId, Stream fileStream, string fileName, string contentType);
    Task<List<TicketAttachmentDto>> GetByTicketAsync(Guid ticketId);
}
