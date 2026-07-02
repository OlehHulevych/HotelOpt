using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Services;

public class MessageService:IMessageService
{
    private readonly IRepository<Message> _repository;

    public MessageService(IRepository<Message> repository)
    {
        _repository = repository;
    }
    public async Task<PaginatedResult<MessageDto>> GetMessagesByProperty(Guid propertyId, int page, int pageSize)
    {
        (List<Message> query, int totalCount) = await _repository.GetByConditionPaginated(m=>m.PropertyId==propertyId,page, pageSize);
        List<MessageDto> list = query.Select(message => new MessageDto(message.Id, message.Content, message.SenderId,
            message.PropertyId, message.CreatedAt)).ToList();
        return new PaginatedResult<MessageDto>(list,totalCount, pageSize, page);
    }
}