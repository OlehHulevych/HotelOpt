using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[Route("api/property")]
[ApiController]
public class PropertyController:ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePropertyDto dto)
    {
        var result = await _propertyService.AddProperty(dto);
        if (!result) return BadRequest("Failed to create new property");
        return Ok("Property was created successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var list = await _propertyService.GetAllProperty(pageSize, page);
        return Ok(new { message = "The properties are fetched successfully", list });
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        PropertyDto property = await _propertyService.GetPropertyById(id);
        return Ok(new {message = "Your property was fetched successfully", property});
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePropertyDto data)
    {
         await _propertyService.UpdateProperty(data);
         return Ok(new {message="Property was updated"});
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _propertyService.DeleteProperty(id);
        if (!result) return BadRequest(new {message = "Failed to delete property"});
        return Ok(new {message="Property was deleted"});
    }
    
    
    
    
}