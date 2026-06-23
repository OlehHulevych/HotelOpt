using HotelOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("/api/task")]
public class HouseKeepingTaskController:ControllerBase
{
    private readonly IHousekeepingTaskService _service;

    public HouseKeepingTaskController(IHousekeepingTaskService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateHousekeepingTaskDto dto)
    {
        var result = await _service.CreateTask(dto);
        if (!result) return BadRequest(new {message = "Failed to create task", result});
        return Ok(new { message = "task was created successfully", result });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateHousekeepingTaskDto dto)
    {
        await _service.UpdateTask(dto);
        return Ok(new { message = $"The product {dto.Id} was updated" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<HouseKeepingTaskDto> tasks = await _service.GetAllTasks();
        return Ok(new {message = "You fetched all tasks successfully", tasks});
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        HouseKeepingTaskDto tasks = await _service.GetTaskById(id);
        return Ok(new {message = $"Task {id} was fetched successfully", tasks});
    }
    [HttpGet("staff/{id}")]
    public async Task<IActionResult> GetAllByAssignedId(Guid id)
    {
        List<HouseKeepingTaskDto> tasks = await _service.GetTaskByAssignedUser(id);
        return Ok(new {message = "You fetched all tasks successfully", tasks});
    }
    
    [HttpGet("property/{id}")]
    public async Task<IActionResult> GetAllByProperty(Guid id)
    {
        List<HouseKeepingTaskDto> tasks = await _service.GetTaskByAssignedUser(id);
        return Ok(new {message = "You fetched all tasks successfully", tasks});
    }

    [HttpPatch("start/{id}")]
    public async Task<IActionResult> Start(Guid id)
    {
        await _service.StartTask(id);
        return Ok(new {message = $"Task {id} is started"});
    }
    
    [HttpPatch("cancel/{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelTask(id);
        return Ok(new {message = $"Task {id} is cancelled"});
    }

    [HttpPatch("complete/{id}")] 
    public async Task<IActionResult> Complete(Guid id)
    {
        await _service.CompleteTask(id);
        return Ok(new {message = $"Task {id} is completed"});
    }

    [HttpPatch("reassign/{id}")]
    public async Task<IActionResult> Reassign([FromQuery] Guid staffId, Guid id)
    {
        await _service.ReassignTask(id, staffId);
        return Ok(new { message = $"The task {id} was reassigned to {staffId} employee" });
    }
    
    
    

}

