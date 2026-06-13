using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

async Task EnsureRolesAsync(WebApplication application)
{
    try
    {
        using var scope = application.Services.CreateScope();
        var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = { "OWNER", "Manager", "Staff" };
        foreach (var role in roles)
        {
            if (!await roleManger.RoleExistsAsync(role)) await roleManger.CreateAsync(new IdentityRole(role));
        }
    }
    catch (Exception ex)
    {
        var logger = application.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to seed roles");
    }
}



