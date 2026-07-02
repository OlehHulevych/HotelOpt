using FluentValidation;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;

namespace HoteOpt.Application.Services;

public class AuthService:IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegistrationDto> _registrationValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    public AuthService(IIdentityService identityService, ITokenService tokenService, IValidator<RegistrationDto> registrationValidator, IValidator<LoginDto> loginValidator)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _registrationValidator = registrationValidator;
        _loginValidator = loginValidator;
    }
    public async Task<bool> Register(RegistrationDto dto)
    {
        var validationResult = await _registrationValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        var result = await _identityService.CreateUser(dto.Name, dto.Surname, dto.Email, dto.Role,dto.Password,dto.TenantId);
        return result;
    }

    public async Task<string> Login(LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        var user = await _identityService.FindByEmail(dto.Email);
        bool checkPassword = await _identityService.CheckPassword(user.Id, dto.Password);
        if (!checkPassword) throw new Exception("Invalid password");
        var token = _tokenService.CreateToken(user.TenantId, user.Id, user.FirstName + " " + user.SecondName, user.Email, user.Role);
        return token;
    }
}