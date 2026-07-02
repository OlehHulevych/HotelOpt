using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Authorize]
[Route("api/fairness")]
[ApiController]
public class StaffFairnessController:ControllerBase
{
    private readonly IStaffFairnessService _service;

    public StaffFairnessController(IStaffFairnessService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStaffFairness()
    {
        var result = await _service.GetStaffFairness();
        return Ok(new {message= "Staff fairness is fetched successfully", result});
    }
    
    
}