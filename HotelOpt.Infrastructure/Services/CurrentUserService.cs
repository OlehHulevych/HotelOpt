using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelOpt.Infrastructure.Services;

public class CurrentUserService:ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService (IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        var claim = _contextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        UserId = Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
    public Guid UserId { get; }
}