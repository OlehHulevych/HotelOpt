using HotelOpt.Application.Exceptions;
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
    public async Task<bool> CreateUser(string firstName, string secondName, string email, UserRole role, string password, Guid tenantId, Guid? PropertyId=null)
    {
        User newUser = new User(firstName, secondName, email, tenantId, role,PropertyId );
        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.Select(e=>e.Description)));
        return true;

    }

    public async Task<UserDto> FindByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.Email == null  ) throw new NotFoundException("User is not found");
        Boolean isBanned = user.LockoutEnabled && user.LockoutEnd >= DateTimeOffset.UtcNow;
        UserDto newDto = new UserDto(user.FirstName, user.LastName, user.Email,user.AvatarUrl,user.Id, user.TenantId, user.Role, isBanned,user.PropertyId);
        return newDto;
    }

    public async Task<bool> CheckPassword(Guid id, string password)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new NotFoundException("User is not found");
        bool checkPassword = await _userManager.CheckPasswordAsync(user, password);
        return checkPassword;
    }

    public async Task UpdateAvatar(Guid id, string url)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new NotFoundException("User is not found");
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
        if (user == null) throw new NotFoundException($"User {userId} is not found");
        var newToken = _tokenService.GenerateRefreshToken();
        user.SetRefreshToken(newToken, DateTimeOffset.UtcNow.AddDays(7));
        await _userManager.UpdateAsync(user);
        return newToken;
    }

    public async Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        
        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.Email == null) throw new UnauthorizedException("Invalid refresh token");
        if (user.RefreshTokenExpiresAt < DateTimeOffset.UtcNow) throw new UnauthorizedException("Refresh token has expired");
        Boolean isBanned = user.LockoutEnabled && user.LockoutEnd >= DateTimeOffset.UtcNow;
        return new UserDto(user.FirstName, user.LastName, user.Email, user.AvatarUrl,user.Id, user.TenantId, user.Role, isBanned,user.PropertyId);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new NotFoundException("User is not found");
        user.RevokeRefreshToken();
        await _userManager.UpdateAsync(user);
    }

    public async Task<string?> GetUserEmailAsync(Guid userId)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || string.IsNullOrEmpty(user.Email)) throw new Exception("The user is not found");
        return user.Email;
    }

    public async Task<List<UserDto>> GetAllStaffAsync(Guid tenantId)
    {
        var users = await _userManager.Users.Where(u => u.TenantId == tenantId).ToListAsync();
        var usersList = users
            .Select(u => new UserDto(u.FirstName, u.LastName, u.Email, u.AvatarUrl,u.Id, u.TenantId, u.Role, u.LockoutEnabled && u.LockoutEnd >= DateTimeOffset.UtcNow,u.PropertyId))
            .ToList();
        return usersList;
    }

    public async Task<UserDto> GetUserById(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new Exception($"The user {userId} is not found");
        var userDto = new UserDto(user.FirstName, user.LastName,user.Email,user.AvatarUrl,user.Id, user.TenantId, user.Role,user.LockoutEnabled && user.LockoutEnd >= DateTimeOffset.UtcNow,user.PropertyId);
        return userDto;
    }

    public async Task UpdateRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new Exception($"The user {userId} is not found");
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) throw new Exception("Failed to update role");
        
    }
    

    public async Task BanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new Exception($"The user {userId} is not found");
        
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new Exception("Failed to ban user");
    }

    public async Task UnBanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.Email == null) throw new Exception($"The user {userId} is not found");
        user.LockoutEnd = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new Exception("Failed to ban user");
    }
}