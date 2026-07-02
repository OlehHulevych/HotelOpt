using System.Security.Claims;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace HotelOpt.Hubs;

public class ChatHub:Hub
{
    private readonly IRepository<Message> _repository;

    public ChatHub(IRepository<Message> repository)
    {
        _repository = repository;
    }
    public async override Task OnConnectedAsync()
    {
        var propertyId = Context.GetHttpContext().Request.Query["propertyId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, $"property-{propertyId}");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string content)
    {
        var senderId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var propertyId = Guid.Parse(Context.GetHttpContext()!.Request.Query["propertyId"]);
        var tenantId = Guid.Parse(Context.User.FindFirst("TenantId")?.Value);
        Message newMessage = new Message(content, senderId,propertyId, tenantId);
        await _repository.Add(newMessage);
        await Clients.Group($"property-{propertyId}").SendAsync("ReceiveMessage", content, senderId);

    }
}