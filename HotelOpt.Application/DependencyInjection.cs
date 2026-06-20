using HotelOpt.Application.Services;
using HoteOpt.Application.Interfaces;
using HoteOpt.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HoteOpt.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IRoomService, RoomService>();
        return services;
    }
    
}