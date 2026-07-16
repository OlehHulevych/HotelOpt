using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly IExportService _exportService;

    public ExportController(IExportService exportService)
    {
        _exportService = exportService;
    }

    [HttpGet("bookings/{propertyId:guid}")]
    public async Task<IActionResult> ExportBookings(Guid propertyId)
    {
        var bytes = await _exportService.ExportBookingsAsync(propertyId);
        return File(bytes, "text/csv", "bookings.csv");
    }

    [HttpGet("invoices")]
    public async Task<IActionResult> ExportInvoices()
    {
        var bytes = await _exportService.ExportInvoicesAsync();
        return File(bytes, "text/csv", "invoices.csv");
    }

    [HttpGet("tasks/{propertyId:guid}")]
    public async Task<IActionResult> ExportTasks(Guid propertyId)
    {
        var bytes = await _exportService.ExportTasksAsync(propertyId);
        return File(bytes, "text/csv", "tasks.csv");
    }
}
