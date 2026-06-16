using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Http;
namespace HotelOpt.Infrastructure.Services;

public class CurrentTenantService:ICurrentTenantService
{
    private readonly IHttpContextAccessor _accessor;
    public Guid TenantId { get; }
    public CurrentTenantService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        var claim = _accessor.HttpContext?.User.FindFirst("TenantId")?.Value;
        TenantId = Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
    
    
    
}