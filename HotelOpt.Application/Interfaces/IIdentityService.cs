using HotelOpt.Domain.Enums;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IIdentityService
{
    public Task<bool> CreateUser(string firstName,string secondName, string email, UserRole role, string password, Guid tenantId, Guid? propertyId=null );
    public Task<UserDto> FindByEmail(string email);
    public Task<bool> CheckPassword(Guid Id, string password);
    public Task UpdateAvatar(Guid id, string url);
    public Task<Dictionary<Guid, string>> GetUserNamesByIds(IEnumerable<Guid> ids);
    
    Task<string> GenerateAndSaveRefreshTokenAsync(Guid userId);
    Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(Guid userId);
    Task<string?> GetUserEmailAsync(Guid userId);
}