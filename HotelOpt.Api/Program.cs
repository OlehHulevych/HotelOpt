using System.Text;
using Hangfire;
using HotelOpt.Handlers;
using HotelOpt.Hubs;
using HotelOpt.Infrastructure;
using HoteOpt.Application;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };

    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p=>p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHub<ChatHub>("/hubs/chat");
RecurringJob.AddOrUpdate<IAutoAssignmentService>("daily-housekeeping-assigment", service=>service.AssignDailyHousekeepingTasks(), Cron.Daily);
app.MapControllers();


async Task EnsureRolesAsync(WebApplication application)
{
    try
    {
        using var scope = application.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        string[] roles = {"Owner", "Manager", "Staff" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
    catch (Exception ex)
    {
        var logger = application.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to seed roles");
    }
}

await EnsureRolesAsync(app);

app.Run();



