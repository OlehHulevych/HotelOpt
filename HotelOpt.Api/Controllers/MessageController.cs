using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Authorize]
[ApiController]
[Route("api/message")]
public class MessageController:ControllerBase
{
    private readonly IMessageService _service;

    public MessageController(IMessageService messageService)
    {
        _service = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMessages( [FromQuery] Guid propertyId, [FromQuery] int pageSize = 10, [FromQuery] int currentPage=1)
    {
        var result = await _service.GetMessagesByProperty(propertyId, currentPage, pageSize);
        return Ok(new {message = "ALl messages were fetched", result});
    }
    
    
    
    
}