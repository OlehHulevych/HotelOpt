using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentTenantService _tenantService;

    public UsersController(IIdentityService identityService, ICurrentTenantService tenantService)
    {
        _identityService = identityService;
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _identityService.GetAllStaffAsync(_tenantService.TenantId);
        return Ok(new { message = "Users fetched successfully", users });
    }
}
