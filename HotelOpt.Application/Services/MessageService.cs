using HotelOpt.Application.DTOs;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;

namespace HotelOpt.Application.Services;

public class MessageService:IMessageService
{
    private readonly IRepository<Message> _repository;
    private readonly IIdentityService _identityService;

    public MessageService(IRepository<Message> repository, IIdentityService identityService)
    {
        _repository = repository;
        _identityService = identityService;
    }
    public async Task<PaginatedResult<MessageDto>> GetMessagesByProperty(Guid propertyId, int page, int pageSize)
    {
        (List<Message> query, int totalCount) = await _repository.GetByConditionPaginated(m=>m.PropertyId==propertyId,page, pageSize);
        var senderIds = query.Select(s => s.SenderId).Distinct().ToList();
        var names = await _identityService.GetUserNamesByIds(senderIds);
        List<MessageDto> list = query.Select(message => new MessageDto(message.Id, message.Content, message.SenderId, names.GetValueOrDefault(message.SenderId, "Unknown"),
            message.PropertyId, message.CreatedAt)).ToList();
        return new PaginatedResult<MessageDto>(list,totalCount, pageSize, page);
    }
}