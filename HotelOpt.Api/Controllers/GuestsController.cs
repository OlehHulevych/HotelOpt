using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/guests")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _service;

    public GuestsController(IGuestService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateGuestDto dto)
    {
        var result = await _service.AddGuest(dto);
        if (!result) return BadRequest(new { message = "Failed to create guest" });
        return Ok(new { message = "Guest was created successfully" });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateGuestDto dto)
    {
        await _service.UpdateGuest(dto);
        return Ok(new { message = $"Guest {dto.Id} was updated" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int currentPage = 1, int pageSize = 10)
    {
        var data = await _service.GetAll(currentPage, pageSize);
        return Ok(new { message = "Guests fetched successfully", data });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var guest = await _service.GetById(id);
        return Ok(new { message = $"Guest {id} fetched successfully", guest });
    }
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteGuest(id);
        if (!result) return BadRequest(new { message = $"Failed to delete guest {id}" });
        return Ok(new { message = "Guest was deleted successfully" });
    }
}
