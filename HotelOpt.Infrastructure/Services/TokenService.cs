using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelOpt.Domain.Enums;
using HoteOpt.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelOpt.Infrastructure.Services;

public class TokenService:ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string CreateToken(Guid tenantId, Guid userId, string name, string email, UserRole role)
    {
        var claimList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("TenantId", tenantId.ToString()),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        

        JwtSecurityToken token = new JwtSecurityToken(
            issuer:_configuration["Jwt:issuer"],
            audience:_configuration["Jwt:audience"],
            claims:claimList,
            expires:DateTime.UtcNow.AddMinutes( Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials:credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}