
using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HoteOpt.Application.Pagination;

namespace HoteOpt.Application.Interfaces;

public interface IMessageService
{
    Task<PaginatedResult<MessageDto>> GetMessagesByProperty(Guid propertyId, int page, int pageSize);
}