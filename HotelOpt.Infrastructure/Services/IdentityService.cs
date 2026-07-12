using HotelOpt.Domain.Enums;
using HotelOpt.Infrastructure.Identity;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelOpt.Infrastructure.Services;

public class IdentityService:IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public IdentityService(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public async Task<bool> CreateUser(string firstName, string secondName, string email, UserRole role, string password, Guid tenantId)
    {
        User newUser = new User(firstName, secondName, email, tenantId, role );
        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.Select(e=>e.Description)));
        return true;

    }

    public async Task<UserDto> FindByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.Email == null  ) throw new Exception("User is not found");
        UserDto newDto = new UserDto(user.FirstName, user.LastName, user.Email,user.Id, user.TenantId, user.Role);
        return newDto;
    }

    public async Task<bool> CheckPassword(Guid id, string password)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new Exception("User is not found");
        bool checkPassword = await _userManager.CheckPasswordAsync(user, password);
        return checkPassword;
    }

    public async Task UpdateAvatar(Guid id, string url)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new Exception("User is not found");
        user.SetAvatar(url);
        await _userManager.UpdateAsync(user);

    }

    public async Task<Dictionary<Guid, string>> GetUserNamesByIds(IEnumerable<Guid> ids)
    {
        var users = await _userManager.Users.Where(u => ids.Contains(u.Id))
            .Select(u => new { u.Id, u.FirstName, u.LastName }).ToDictionaryAsync(u=>u.Id, u=>$"{u.FirstName} {u.LastName}");
        return users;

    }

    public async Task<string> GenerateAndSaveRefreshTokenAsync(Guid userId)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new Exception($"The user {userId} is not found");
        var newToken = _tokenService.GenerateRefreshToken();
        user.SetRefreshToken(newToken, DateTimeOffset.UtcNow.AddDays(7));
        await _userManager.UpdateAsync(user);
        return newToken;
    }

    public async Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        
        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.Email == null) throw new Exception("User is not found");
        if (user.RefreshTokenExpiresAt < DateTimeOffset.UtcNow) throw new Exception("Refresh token has expired");
        return new UserDto(user.FirstName, user.LastName, user.Email, user.Id, user.TenantId, user.Role);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new Exception("The user is not found");
        user.RevokeRefreshToken();
        await _userManager.UpdateAsync(user);
    }
}