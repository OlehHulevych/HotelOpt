using HoteOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;

namespace HoteOpt.Application.Services;

public class AuthService:IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    public AuthService(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }
    public async Task<bool> Register(RegistrationDto dto)
    {
        var result = await _identityService.CreateUser(dto.Name, dto.Surname, dto.Email, dto.Role,dto.Password,dto.TenantId);
        return true;
    }

    public async Task<string> Login(LoginDto dto)
    {
        var user = await _identityService.FindByEmail(dto.Email);
        bool checkPassword = await _identityService.CheckPassword(user.Id, dto.Password);
        if (!checkPassword) throw new Exception("Invalid password");
        var token = _tokenService.CreateToken(user.TenantId, user.Id, user.FirstName + " " + user.SecondName, user.Email, user.Role);
        return token;
    }
}