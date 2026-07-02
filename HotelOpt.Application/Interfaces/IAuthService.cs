using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IAuthService
{
    public Task<bool> Register(RegistrationDto dto);
    public Task<string> Login(LoginDto dto);
}