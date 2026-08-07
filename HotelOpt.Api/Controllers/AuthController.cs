using System.Security.Claims;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Route("/api/auth")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IFileStorageService _storageService;
    private readonly IIdentityService _identityService;
    

    public AuthController(IAuthService authService, IFileStorageService storageService, IIdentityService identityService)
    {
        _authService = authService;
        _storageService = storageService;
        _identityService = identityService;
    }
        
    [Authorize(Roles="Owner")]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto data)
    {
        var result = await _authService.Register(data);
        if (!result) return BadRequest("Failed to register user");
        return Ok("User is registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto data)
    {
        AuthResponseDto responseDto = await _authService.Login(data);
        return Ok(new { message = "The user is log in successfully ", responseDto });

    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var response = await _authService.RefreshAsync(refreshToken);
        return Ok(new { message = "Token refreshed successfully", response });
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _authService.RevokeRefreshTokenAsync(userId);
        return Ok(new { message = "Token revoked successfully" });
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        var url = await _storageService.UploadAsync(file.OpenReadStream(), file.FileName, file.ContentType, "avatars");
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (value != null)
        {
            var userId  = Guid.Parse(value);
            await _identityService.UpdateAvatar(userId,url);
        }

        return Ok(new {message = "Your avatar was uploaded"});
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _authService.Authenticate(id);
        return Ok(user);
    }
    
    
    

}