using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/tickets/{ticketId:guid}/attachments")]
public class TicketAttachmentController : ControllerBase
{
    private readonly ITicketAttachmentService _service;

    public TicketAttachmentController(ITicketAttachmentService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> Upload(Guid ticketId, [FromForm] IFormFile file)
    {
        var result = await _service.UploadAsync(ticketId, file.OpenReadStream(), file.FileName, file.ContentType);
        return Ok(new { message = "Attachment uploaded successfully", data = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetByTicket(Guid ticketId)
    {
        var result = await _service.GetByTicketAsync(ticketId);
        return Ok(new { message = "Attachments fetched successfully", data = result });
    }
}
