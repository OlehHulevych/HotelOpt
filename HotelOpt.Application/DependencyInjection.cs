using FluentValidation;
using HotelOpt.Application.Services;
using HotelOpt.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HotelOpt.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IHousekeepingTaskService, HousekeepingTaskService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<IAutoAssignmentService, AutoAssignmentsService>();
        services.AddScoped<IMaintenanceTicketService, MaintenanceTicketService>();
        services.AddScoped<IMessageService, MessageService > ();
        services.AddScoped<ISmartAlertService, SmartAlertService>();
        services.AddScoped<IStaffFairnessService, StaffFairnessService>();
        services.AddScoped<IRoomPhotoService, RoomPhotoService>();
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ITicketAttachmentService, TicketAttachmentService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ITaskTemplateService, TaskTemplateService>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
    
}