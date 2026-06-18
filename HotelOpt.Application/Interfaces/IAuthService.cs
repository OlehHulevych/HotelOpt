using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IAuthService
{
    public Task<bool> Register(RegistrationDto dto);
    public Task<string> Login(LoginDto dto);
}