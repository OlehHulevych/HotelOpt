using HotelOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HotelOpt.Controllers;
[Authorize]
[ApiController]
[Route("api/tickets")]
public class MaintenanceController:ControllerBase
{
    private readonly IMaintenanceTicketService _service;

    public MaintenanceController(IMaintenanceTicketService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateMaintenanceTicketDto dto)
    {
        var result = await _service.AddTicket(dto);
        if (!result) return BadRequest(new {message = "Failed to create task", result});
        return Ok(new { message = "ticket was created successfully", result });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateMaintenanceTicketDto dto)
    {
        await _service.UpdateTask(dto);
        return Ok(new { message = $"The ticket {dto.Id} was updated" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int currentPage = 1, int pageSize = 10)
    {
        var data = await _service.GetAll(currentPage, pageSize);
        return Ok(new {message = "You fetched all tickets successfully", data});
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        MaintenanceTicketDto tasks = await _service.GetById(id);
        return Ok(new {message = $"Ticket {id} was fetched successfully", tasks});
    }
    [HttpGet("staff/{id}")]
    public async Task<IActionResult> GetAllByAssignedId(Guid id, [FromQuery] int currentPage = 1, int pageSize= 10)
    {
        var data = await _service.GetByStaffId(id, currentPage, pageSize);
        return Ok(new {message = "You fetched all tickets successfully", data});
    }
    
    [HttpGet("property/{id}")]
    public async Task<IActionResult> GetAllByProperty(Guid id, [FromQuery] int currentPage = 1, int pageSize = 10)
    {
        var data  = await _service.GetAllByProperty(id, currentPage, pageSize);
        return Ok(new {message = "You fetched all tickets successfully", data});
    }
    [HttpGet("room/{id}")]
    public async Task<IActionResult> GetAllByRoom(Guid id, [FromQuery] int currentPage = 1, int pageSize = 10)
    {
        var data  = await _service.GetAllByRoom(id, currentPage, pageSize);
        return Ok(new {message = "You fetched all tickets successfully", data});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResult(Guid id)
    {
        var result = await _service.DeleteTask(id);
        if (!result) return BadRequest(new {message = $"Failed to delete ticket {id}",result});
        return Ok(new {message = $"Ticket was deleted successfully", result});
        
    }

    [HttpPatch("resolve/{id}")]
    public async Task<IActionResult> Resolve(Guid id)
    {
        await _service.Resolve(id);
        return Ok(new {message = $"The ticket {id} is resolved"});
    }
    
    [HttpPatch("close/{id}")]
    public async Task<IActionResult> Close(Guid id)
    {
        await _service.Close(id);
        return Ok(new {message = $"The ticket {id} is closed"});
    }
    

    [HttpPatch("reassign/{id}")]
    public async Task<IActionResult> Reassign([FromQuery] Guid staffId, Guid id)
    {
        await _service.Reassign(id, staffId);
        return Ok(new { message = $"The ticket {id} was reassigned to {staffId} employee" });
    }
}