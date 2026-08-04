using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;
[Authorize]
[ApiController]
[Route("api/translate")]
public class DeepLController:ControllerBase
{
    private readonly ITranslationService _service;

    public DeepLController(ITranslationService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> TranslateText([FromBody] TranslateRequestDto requestDto)
    {
        var text = await _service.TranslateAsync(requestDto.Text, requestDto.TargetLanguage);
        return Ok(new TranslateResponseDto(text));
    }
}