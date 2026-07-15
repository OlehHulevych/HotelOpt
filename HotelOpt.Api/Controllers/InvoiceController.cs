using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetByBooking(Guid bookingId)
    {
        var invoice = await _invoiceService.GetByBookingAsync(bookingId);
        return Ok(new { message = "Invoice fetched successfully", invoice });
    }
}
