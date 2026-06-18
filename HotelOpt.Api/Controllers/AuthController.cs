using HoteOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Route("/api/auth")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
    
    
    

}