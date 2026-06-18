using HotelOpt.Domain.Enums;
using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IIdentityService
{
    public Task<bool> CreateUser(string firstName,string secondName, string email, UserRole role, string password, Guid tenantId );
    public Task<UserDto> FindByEmail(string email);
    public Task<bool> CheckPassword(Guid Id, string password);
}