using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/task-templates")]
public class TaskTemplateController : ControllerBase
{
    private readonly ITaskTemplateService _service;

    public TaskTemplateController(ITaskTemplateService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskTemplateDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(new { message = "Task template created successfully" });
    }

    [HttpGet("property/{propertyId:guid}")]
    public async Task<IActionResult> GetByProperty(Guid propertyId)
    {
        var templates = await _service.GetByPropertyAsync(propertyId);
        return Ok(new { message = "Task templates fetched successfully", data = templates });
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{templateId:guid}/apply")]
    public async Task<IActionResult> Apply(Guid templateId, [FromBody] ApplyTemplateDto dto)
    {
        await _service.ApplyAsync(templateId, dto);
        return Ok(new { message = "Template applied successfully" });
    }
}
