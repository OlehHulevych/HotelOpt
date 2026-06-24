using HotelOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Authorize]
[Route("api/room")]
[ApiController]
public class RoomController:ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateRoomDto dto)
    {
        var result = await _roomService.AddRoom(dto);
        if (!result) return BadRequest("Failed to create new room");
        return Ok("Room was created successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]int currentPage = 1 , [FromQuery] int pageSize = 10)
    {
        var list = await _roomService.GetAllRooms(pageSize,currentPage);
        return Ok(new { message = "The rooms are fetched successfully", list });
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        RoomDto room = await _roomService.GetRoomById(id);
        if (room == null) return BadRequest(new {message=$"Failed to get your room {id}"});
        return Ok(new {message = "Your room was fetched successfully", room});
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoomDto data)
    {
        await _roomService.UpdateRoom(data);
        return Ok(new {message="room was updated"});
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _roomService.DeleteRoom(id);
        if (!result) return BadRequest(new {message = "Failed to delete room"});
        return Ok(new {message="Room was deleted"});
    }
}