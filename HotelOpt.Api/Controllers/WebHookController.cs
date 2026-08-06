using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[ApiController]
[Route("api/subscription")]
public class WebHookController:ControllerBase
{
    private ISubscriptionService _service;
    private ICurrentTenantService _tenantService;
    private IConfiguration _configuration;
    public WebHookController(ISubscriptionService service, ICurrentTenantService tenantService, IConfiguration configuration)
    {
        _service = service;
        _tenantService = tenantService;
        _configuration = configuration;
    }
    [Authorize]
    [HttpPost]
    [Route("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscriptionPlan plan, string priceId)
    {
        await _service.SubscribeAsync(_tenantService.TenantId,plan, priceId);
        return Ok(new {message="Subscription is updated"});
    }
    
    
    [HttpPost]
    [Route("webhook")]
    public async Task<IActionResult> WebHook()
    {
        try
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            string json = await reader.ReadToEndAsync();
            string signature = Request.Headers["Stripe-Signature"]!;
            string webhookSecret = _configuration["Stripe:WebhookSecret"]!;
            await _service.HandleWebhookAsync(json, signature, webhookSecret);
            return Ok(new { message = "WebHook is verified" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WEBHOOK ERROR: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> GetStatusAsync()
    {
        var result = await _service.GetStatusAsync();
        return Ok(new { message = "Subscription Status", result });
    }

    [Authorize(Roles="Owner")]
    [HttpDelete]
    public async Task<IActionResult> Cancel()
    {
         await _service.CancelAsync();
         return Ok(new {message="Subscription is cancelled"});
    }
    
    
}