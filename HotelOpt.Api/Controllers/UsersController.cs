using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

public record UpdateRoleRequest(string Role);

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

    [Authorize(Roles = "Owner")]
    [HttpPatch("{id}/role")]
    public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var target = await _identityService.GetUserById(id);
        if (target.TenantId != _tenantService.TenantId)
            return Forbid();

        await _identityService.UpdateRoleAsync(id, request.Role);
        return Ok(new { message = $"The user {id} was updated role on {request.Role}" });
    }

    [Authorize(Roles = "Owner")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> BanUser(Guid id)
    {
        var target = await _identityService.GetUserById(id);
        if (target.TenantId != _tenantService.TenantId)
            return Forbid();

        await _identityService.BanUserAsync(id);
        return Ok(new { message = $"The user {id} was banned" });
    }
}
