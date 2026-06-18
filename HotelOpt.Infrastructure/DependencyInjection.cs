using HotelOpt.Infrastructure.Data;
 using HotelOpt.Infrastructure.Identity;
 using HotelOpt.Infrastructure.Services;
 using HoteOpt.Application.Interfaces;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Configuration;
 using Microsoft.Extensions.DependencyInjection;
 
 namespace HotelOpt.Infrastructure;
 
 public static class DependencyInjection
 {
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
     {
         services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
         services.AddScoped<ICurrentTenantService, CurrentTenantService>();
         services.AddIdentityCore<User>().AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();
         services.AddTransient<ITokenService, TokenService>();
         
         return services;
     }
 }