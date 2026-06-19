using HotelOpt.Domain.Enums;
using HotelOpt.Infrastructure.Identity;
using HoteOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace HotelOpt.Infrastructure.Services;

public class IdentityService:IIdentityService
{
    private readonly UserManager<User> _userManager;

    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
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
        if (user == null  ) throw new Exception("User is not found");
        UserDto newDto = new UserDto(user.FirstName, user.LastName, user.Email,user.Id, user.TenantId, user.Role);
        return newDto;
    }

    public async Task<bool> CheckPassword(Guid Id, string password)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());
        if (user == null) throw new Exception("User is not found");
        bool checkPassword = await _userManager.CheckPasswordAsync(user, password);
        return checkPassword;
    }
}