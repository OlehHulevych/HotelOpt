using HotelOpt.Domain.Enums;

namespace HoteOpt.Application.Interfaces;

public interface ITokenService
{
    public string CreateToken(Guid tenantId, Guid userId,string name, string email, UserRole role);
}