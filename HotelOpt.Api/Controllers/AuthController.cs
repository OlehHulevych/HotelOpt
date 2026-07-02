using System.Security.Claims;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
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
        var token = await _authService.Login(data);
        if (String.IsNullOrEmpty(token)) return BadRequest("Failed to login user");
        return Ok(new { message = "The user is log in successfully ", token });

    }
    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        var url = await _storageService.UploadAsync(file.OpenReadStream(), file.FileName, file.ContentType, "avatars");
        var userId  = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _identityService.UpdateAvatar(userId,url);
        return Ok(new {message = "Your avatar was uploaded"});
    }
    
    
    

}