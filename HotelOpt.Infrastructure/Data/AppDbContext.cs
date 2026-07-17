using HotelOpt.Domain.Entities;
using HotelOpt.Infrastructure.Identity;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotelOpt.Infrastructure.Data;

public class AppDbContext:IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<HouseKeepingTask> HouseKeepingTasks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<MaintenanceTicket> MaintenanceTickets { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<RoomInspection> RoomInspections { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<RoomPhoto> RoomPhotos { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    private ICurrentTenantService _currentTenantService;
    private ICurrentUserService? _currentUserService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentTenantService currentTenantService, ICurrentUserService currentUserService) : base(options)
    {
        _currentTenantService = currentTenantService;
        _currentUserService = currentUserService;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService != null)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                if (entry.State == EntityState.Added)
                {
                    var changes = entry.Properties.ToDictionary(
                        p => p.Metadata.Name,
                        p => new { From = p.OriginalValue, To = p.CurrentValue }
                    );
                    string changesJson = JsonSerializer.Serialize(changes);
                    AuditLog newAuditLog = new AuditLog(entry.Entity.GetType().Name, entry.Entity!.Id, EntityAction.Created,
                        _currentUserService.UserId, _currentTenantService.TenantId, changesJson);
                    await AuditLogs.AddAsync(newAuditLog);
                }

                if (entry.State == EntityState.Modified)
                {
                    var changes = entry.Properties.Where(p => p.IsModified)
                        .ToDictionary(
                            p => p.Metadata.Name,
                            p => new { From = p.OriginalValue, To = p.CurrentValue }
                        );
                    string changesJson = JsonSerializer.Serialize(changes);
                    AuditLog newAuditLog = new AuditLog(entry.Entity.GetType().Name, entry.Entity!.Id, EntityAction.Updated,
                        _currentUserService.UserId, _currentTenantService.TenantId, changesJson);
                    await AuditLogs.AddAsync(newAuditLog);
                }

                if (entry.State == EntityState.Deleted)
                {
                    var deleted = new { Name = entry.Entity.GetType().Name, };
                    string changesJson = JsonSerializer.Serialize(deleted);
                    AuditLog newAuditLog = new AuditLog(entry.Entity.GetType().Name, entry.Entity!.Id, EntityAction.Deleted,
                        _currentUserService.UserId, _currentTenantService.TenantId, changesJson);
                    await AuditLogs.AddAsync(newAuditLog);
                }

            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<TicketAttachment>()
            .HasQueryFilter(ta => ta.TenantId == _currentTenantService.TenantId && !ta.IsDeleted);
        builder.Entity<AuditLog>().HasQueryFilter(b => b.TenantId == _currentTenantService.TenantId && !b.IsDeleted);
        builder.Entity<Booking>().HasQueryFilter(b => b.TenantId == _currentTenantService.TenantId && !b.IsDeleted);
        builder.Entity<Guest>().HasQueryFilter(g => g.TenantId == _currentTenantService.TenantId && !g.IsDeleted);
        builder.Entity<RoomPhoto>().HasQueryFilter(rp=>rp.TenantId == _currentTenantService.TenantId && !rp.IsDeleted);
        builder.Entity<User>().HasQueryFilter(u =>_currentTenantService.TenantId == Guid.Empty || u.TenantId == _currentTenantService.TenantId);
        builder.Entity<RoomInspection>().HasQueryFilter(i=>i.TenantId==_currentTenantService.TenantId && !i.IsDeleted);
        builder.Entity<Tenant>().HasQueryFilter(t => t.Id == _currentTenantService.TenantId && !t.IsDeleted);
        builder.Entity<Property>().HasQueryFilter(p => p.TenantId == _currentTenantService.TenantId && !p.IsDeleted);
        builder.Entity<Room>().HasQueryFilter(r => r.TenantId == _currentTenantService.TenantId && !r.IsDeleted);
        builder.Entity<HouseKeepingTask>().HasQueryFilter(t => t.TenantId == _currentTenantService.TenantId && !t.IsDeleted);
        builder.Entity<Shift>().HasQueryFilter(s => s.TenantId == _currentTenantService.TenantId && !s.IsDeleted);
        builder.Entity<MaintenanceTicket>().HasQueryFilter(t => t.TenantId == _currentTenantService.TenantId && !t.IsDeleted);
        builder.Entity<Message>().HasQueryFilter(m => m.TenantId == _currentTenantService.TenantId && !m.IsDeleted);
        builder.Entity<Invoice>().HasQueryFilter(i => i.TenantId == _currentTenantService.TenantId && !i.IsDeleted);

        builder.Entity<Property>().HasOne(p => p.Tenant)
            .WithMany().HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Room>().HasOne(r => r.Property).WithMany().HasForeignKey(r=>r.PropertyId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<HouseKeepingTask>().HasOne(t => t.Room).WithMany().HasForeignKey(t=>t.RoomId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Shift>().HasOne(s=>s.Property).WithMany().HasForeignKey(s=>s.PropertyId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Shift>().HasOne(s=>s.Tenant).WithMany().HasForeignKey(s=>s.TenantId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RoomPhoto>().HasOne(rp => rp.Room).WithMany(r => r.Photos).HasForeignKey(rp => rp.RoomId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RoomInspection>().HasOne(i => i.Room).WithMany().HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RoomInspection>().HasOne(i => i.Property).WithMany().HasForeignKey(i => i.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Booking>().HasOne(b => b.Property).WithMany().HasForeignKey(b => b.PropertyId);
        builder.Entity<Booking>().HasOne(b => b.Room).WithMany().HasForeignKey(b=>b.RoomId);
        builder.Entity<Booking>().HasMany(b => b.Guests).WithMany(g => g.Bookings);
        builder.Entity<Invoice>().HasOne(i => i.Booking).WithMany().HasForeignKey(i=>i.BookingId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Invoice>().HasOne(i => i.Room).WithMany().HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<TicketAttachment>().HasOne(t => t.Ticket).WithMany().HasForeignKey(t => t.TicketId)
            .OnDelete(DeleteBehavior.Cascade);



    }
}