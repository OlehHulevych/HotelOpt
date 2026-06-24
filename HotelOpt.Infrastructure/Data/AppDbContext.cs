using HotelOpt.Domain.Entities;
using HotelOpt.Infrastructure.Identity;
using HoteOpt.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelOpt.Infrastructure.Data;

public class AppDbContext:IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<HouseKeepingTask> HouseKeepingTasks { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    private ICurrentTenantService _currentTenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Tenant>().HasQueryFilter(t => t.Id == _currentTenantService.TenantId);
        builder.Entity<Property>().HasQueryFilter(p => p.TenantId == _currentTenantService.TenantId);
        builder.Entity<Room>().HasQueryFilter(r => r.TenantId == _currentTenantService.TenantId);
        builder.Entity<HouseKeepingTask>().HasQueryFilter(t => t.TenantId == _currentTenantService.TenantId);
        builder.Entity<Shift>().HasQueryFilter(s => s.TenantId == _currentTenantService.TenantId);

        builder.Entity<Property>().HasOne(p => p.Tenant)
            .WithMany().HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Room>().HasOne(r => r.Property).WithMany().HasForeignKey(r=>r.PropertyId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<HouseKeepingTask>().HasOne(t => t.Room).WithMany().HasForeignKey(t=>t.RoomId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Shift>().HasOne(s=>s.Property).WithMany().HasForeignKey(s=>s.PropertyId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Shift>().HasOne(s=>s.Tenant).WithMany().HasForeignKey(s=>s.TenantId).OnDelete(DeleteBehavior.Cascade);
        
    }
}