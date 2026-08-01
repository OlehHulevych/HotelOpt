using System.Security.Claims;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace HotelOpt.Hubs;

public class ChatHub:Hub
{
    private readonly IRepository<Message> _repository;
    private readonly IIdentityService _identityService;

    public ChatHub(IRepository<Message> repository, IIdentityService identityService)
    {
        _repository = repository;
        _identityService = identityService;
    }
    public async override Task OnConnectedAsync()
    {
        var propertyId = Context.GetHttpContext()!.Request.Query["propertyId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, $"property-{propertyId}");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string content)
    {
        var senderId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var propertyId = Guid.Parse(Context.GetHttpContext()!.Request.Query["propertyId"]);
        var tenantId = Guid.Parse(Context.User.FindFirst("TenantId")?.Value);
        IList<Guid> idList =  new List<Guid>{senderId};
        var names = await _identityService.GetUserNamesByIds(idList);
        var senderName = names.GetValueOrDefault(senderId,"Unknown");
        Message newMessage = new Message(content, senderId,propertyId, tenantId);
        await _repository.Add(newMessage);
        await Clients.Group($"property-{propertyId}").SendAsync("ReceiveMessage", content, senderId, senderName);

    }
}