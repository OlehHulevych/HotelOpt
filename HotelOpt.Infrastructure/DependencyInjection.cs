using Azure.Storage.Blobs;
using Hangfire;
using Hangfire.PostgreSql;
using HotelOpt.Application.Interfaces;
using HotelOpt.Infrastructure.Data;
 using HotelOpt.Infrastructure.Identity;
 using HotelOpt.Infrastructure.Repositories;
 using HotelOpt.Infrastructure.Services;
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
         services.AddScoped<ICurrentUserService, CurrentUserService>();
         services.AddIdentityCore<User>().AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();
         services.AddTransient<ITokenService, TokenService>();
         services.AddScoped<IIdentityService, IdentityService>();
         services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
         services.AddScoped<IFileStorageService, FileStorageService>();
         services.AddScoped<IGeminiService, GeminiService>();
         services.AddScoped<IRoomInspectionService, RoomInspectionService>();
         services.AddHttpClient("gemini");
         services.AddHangfire(config =>
             config.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
         services.AddHangfireServer();
         services.AddSingleton(x => new BlobServiceClient(configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
         return services;
     }
 }