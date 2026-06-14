using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HotelOpt.Infrastructure.Identity;

public class User : IdentityUser<Guid>
{
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public UserRole Role { get; private set; }

    private User()
    {
        TenantId = Guid.Empty;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        Tenant = null!;

    }

    public User(Guid tenantId, UserRole role)
    {
        TenantId = tenantId;
        Role = role;
        Tenant = null!;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}