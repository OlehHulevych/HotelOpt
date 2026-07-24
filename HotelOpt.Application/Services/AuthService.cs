using FluentValidation;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Application.Services;

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
        var result = await _identityService.CreateUser(dto.Name, dto.Surname, dto.Email, dto.Role,dto.Password,dto.TenantId,dto.PropertyId);
        return result;
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        var user = await _identityService.FindByEmail(dto.Email);
        UserDto userDto = new UserDto(user.FirstName,user.SecondName,user.Email, user.Id,user.TenantId,user.Role,user.PropertyId);
        bool checkPassword = await _identityService.CheckPassword(user.Id, dto.Password);
        if (!checkPassword) throw new Exception("Invalid password");
        var refreshToken = await _identityService.GenerateAndSaveRefreshTokenAsync(user.Id);
        var token = _tokenService.CreateToken(user.TenantId, user.Id, user.FirstName + " " + user.SecondName, user.Email, user.Role, user.PropertyId);
        return new AuthResponseDto(token, refreshToken,userDto);
    }

    public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
    {
        var user = await _identityService.GetUserByRefreshTokenAsync(refreshToken);
        UserDto userDto = new UserDto(user.FirstName,user.SecondName,user.Email, user.Id,user.TenantId,user.Role, user.PropertyId);
        var newRefreshToken = await _identityService.GenerateAndSaveRefreshTokenAsync(user.Id);
        var newAccessToken =  _tokenService.CreateToken(user.TenantId, user.Id, user.FirstName + " "+user.SecondName, user.Email,user.Role,user.PropertyId);
        return new AuthResponseDto(newAccessToken, newRefreshToken, userDto);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        await _identityService.RevokeRefreshTokenAsync(userId);
    }
}