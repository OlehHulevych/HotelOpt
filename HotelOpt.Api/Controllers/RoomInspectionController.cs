using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[Route("api/inspections")]
[ApiController]
public class RoomInspectionController:ControllerBase
{
    private readonly IGeminiService _geminiService;
    private readonly IFileStorageService _fileService;
    private readonly IRoomInspectionService _inspectionService;
    private readonly ICurrentUserService _currentUserService;

    public RoomInspectionController(IGeminiService geminiService, IRoomInspectionService inspectionService, ICurrentUserService currentUserService, IFileStorageService fileService)
    {
        _geminiService = geminiService;
        _inspectionService = inspectionService;
        _currentUserService = currentUserService;
        _fileService = fileService;
    }
    
    [HttpPost]
    public async Task<IActionResult> InspectRoom([FromForm] IFormFile file, [FromQuery] Guid roomId, [FromQuery] Guid propertyId)
    {
        var url = await _fileService.UploadAsync(file.OpenReadStream(),file.FileName,file.ContentType,"inspections");
        var resultAi = await _geminiService.InspectRoom(url, file.ContentType);
        await _inspectionService.CreateInspectionAsync(roomId, propertyId, _currentUserService.UserId, url, resultAi);
        return Ok(resultAi);
    }

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetResult(Guid roomId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)

    {
        var result = await _inspectionService.GetInspectionsByRoomAsync(roomId, page,pageSize);
        return Ok(result);
    }
}