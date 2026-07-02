using HotelOpt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelOpt.Controllers;

[Authorize]
[Route("api/rooms/{roomId}/photos")]
[ApiController]
public class RoomPhotoController : ControllerBase
{
    private readonly IRoomPhotoService _roomPhotoService;
    private readonly IFileStorageService _fileService;
    private readonly ICurrentUserService _currentUserService;

    public RoomPhotoController(IRoomPhotoService roomPhotoService, IFileStorageService fileService, ICurrentUserService currentUserService)
    {
        _roomPhotoService = roomPhotoService;
        _fileService = fileService;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhoto(Guid roomId, [FromForm] IFormFile file)
    {
        var url = await _fileService.UploadAsync(file.OpenReadStream(), file.FileName, file.ContentType, "room-photos");
        var photo = await _roomPhotoService.UploadPhotoAsync(roomId, _currentUserService.UserId, url);
        return Ok(photo);
    }

    [HttpGet]
    public async Task<IActionResult> GetPhotos(Guid roomId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _roomPhotoService.GetPhotosByRoomAsync(roomId, page, pageSize);
        return Ok(result);
    }
}
