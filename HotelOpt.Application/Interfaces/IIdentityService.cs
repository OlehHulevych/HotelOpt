using HotelOpt.Domain.Enums;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IIdentityService
{
    public Task<bool> CreateUser(string firstName,string secondName, string email, UserRole role, string password, Guid tenantId );
    public Task<UserDto> FindByEmail(string email);
    public Task<bool> CheckPassword(Guid Id, string password);
    public Task UpdateAvatar(Guid id, string url);
}