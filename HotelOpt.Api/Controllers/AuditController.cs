using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditLogService _service;

    public AuditController(IAuditLogService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var logs = await _service.GetAllAsync();
        return Ok(new { message = "Audit logs fetched successfully", data = logs });
    }

    [HttpGet("{entityName}/{entityId:guid}")]
    public async Task<IActionResult> GetByEntity(string entityName, Guid entityId)
    {
        var logs = await _service.GetByEntityAsync(entityName, entityId);
        return Ok(new { message = "Audit logs fetched successfully", data = logs });
    }
}
