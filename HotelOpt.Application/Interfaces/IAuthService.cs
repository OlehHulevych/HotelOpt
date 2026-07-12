using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IAuthService
{
    public Task<bool> Register(RegistrationDto dto);
    public Task<AuthResponseDto> Login(LoginDto dto);
    public Task<AuthResponseDto> RefreshAsync(string refreshToken);
    public Task RevokeRefreshTokenAsync(Guid userId);
}