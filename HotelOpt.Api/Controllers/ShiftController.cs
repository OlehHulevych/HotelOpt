using HotelOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/shift")]
public class ShiftController:ControllerBase
{
    private readonly IShiftService _service;
    

    public ShiftController(IShiftService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateShiftDto dto)
    {
        bool result = await _service.AddShift(dto);
        if (!result) return BadRequest(new {message = "Failed to create new shift", result });
        return Ok(new {message = "The shift was created", result});
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateShiftDto dto)
    {
        await _service.UpdateShift(dto);
        return Ok(new { message = "shift was updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteShift(id);
        if (!result) return BadRequest(new {message = $"Failed to delete shift {id}", result});
        return Ok(new {message = $"Shift {id} was deleted"});
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var shift = await _service.GetShiftById(id);
        return Ok(new {message = $"The shift {id} was retrieved", shift});
        
    } 

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
    {
        var list = await _service.GetAllShifts(currentPage, pageSize);
        return Ok(new { message = "All shifts were fetched", list });
    }
    
    [HttpGet("property/{id}")]
    public async Task<IActionResult> GetAllByProperty(Guid id, [FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
    {
        var list = await _service.GetAllShiftsByProperty(id, currentPage, pageSize);
        return Ok(new { message = "All shifts were fetched", list });
    }
    [HttpGet("staff/{id}")]
    public async Task<IActionResult> GetAllByStaff(Guid id, [FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
    {
        var list = await _service.GetShiftByStaff(id, currentPage, pageSize);
        return Ok(new { message = "All shifts were fetched", list });
    }
    
    
    
}