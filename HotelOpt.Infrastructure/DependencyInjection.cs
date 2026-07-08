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
using Stripe;
using IdentityService = HotelOpt.Infrastructure.Services.IdentityService;
using TokenService = HotelOpt.Infrastructure.Services.TokenService;


namespace HotelOpt.Infrastructure;
 
 public static class DependencyInjection
 {
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
     {
         StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
         services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
         services.AddScoped<ICurrentTenantService, CurrentTenantService>();
         services.AddScoped<ICurrentUserService, CurrentUserService>();
         services.AddIdentityCore<User>().AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();
         services.AddTransient<ITokenService, TokenService>();
         services.AddScoped<IIdentityService, IdentityService>();
         services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
         services.AddScoped<IBookingRepository, BookingRepository>();
         services.AddScoped<IFileStorageService, FileStorageService>();
         services.AddScoped<IGeminiService, GeminiService>();
         services.AddScoped<IRoomInspectionService, RoomInspectionService>();
         services.AddScoped<IStripeService, StripeService>();
         services.AddHttpClient("gemini");
         services.AddHangfire(config =>
             config.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
         services.AddHangfireServer();
         services.AddSingleton(x => new BlobServiceClient(configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
         return services;
     }
 }