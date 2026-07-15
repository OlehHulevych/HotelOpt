using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(new { message = "Booking created successfully", data = result });
    }

    [HttpPost("{id:guid}/checkin")]
    public async Task<IActionResult> CheckIn(Guid id)
    {
        await _service.CheckInAsync(id);
        return Ok(new { message = $"Booking {id} checked in" });
    }

    [HttpPost("{id:guid}/checkout")]
    public async Task<IActionResult> CheckOut(Guid id)
    {
        await _service.CheckOutAsync(id);
        return Ok(new { message = $"Booking {id} checked out" });
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelAsync(id);
        return Ok(new { message = $"Booking {id} cancelled" });
    }

    [HttpPost("{id:guid}/guests/{guestId:guid}")]
    public async Task<IActionResult> AddGuest(Guid id, Guid guestId)
    {
        await _service.AddGuestAsync(id, guestId);
        return Ok(new { message = $"Guest {guestId} added to booking {id}" });
    }

    [HttpGet("property/{propertyId:guid}")]
    public async Task<IActionResult> GetByProperty(Guid propertyId,[FromQuery] BookingFilterDto filters, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetByPropertyAsync(propertyId, page, pageSize,filters);
        return Ok(new { message = "Bookings fetched successfully", data = result });
    }
}
