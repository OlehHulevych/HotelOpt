using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Handlers;

public class SubscriptionMiddleware
{
   private readonly RequestDelegate _next;
   private readonly IServiceScopeFactory _scopeFactory;

   public SubscriptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
   {
      _next = next;
      _scopeFactory = scopeFactory;
   }

   public async Task InvokeAsync(HttpContext context)
   {
       string? tenantId = context.User.FindFirst("TenantId")?.Value;
       if (string.IsNullOrEmpty(tenantId))
       {
           await _next(context);
           return;
       }

       await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
       IRepository<Tenant> repository = scope.ServiceProvider.GetRequiredService<IRepository<Tenant>>();
       Tenant? tenant = await repository.GetById(Guid.Parse(tenantId));
       if (tenant == null)
       {
           await _next(context);
           return;
       }
       if (tenant.SubscriptionStatus == SubscriptionStatus.Locked ||
           tenant.SubscriptionStatus == SubscriptionStatus.Cancelled) 
       {
           context.Response.StatusCode = 402;                                                                                                                                                                                                
           await context.Response.WriteAsync("Your subscription is locked or cancel. Please update your payment.");                                                                                                                                    
           return;        
       }
       await _next(context);
   }
}