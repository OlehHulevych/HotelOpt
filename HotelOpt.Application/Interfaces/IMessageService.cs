
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Interfaces;

public interface IMessageService
{
    Task<PaginatedResult<MessageDto>> GetMessagesByProperty(Guid propertyId, int page, int pageSize);
}