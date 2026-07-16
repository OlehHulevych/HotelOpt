using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class TicketAttachmentService:ITicketAttachmentService
{
    private readonly IRepository<TicketAttachment> _repository;
    private readonly IFileStorageService _storageService;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly ICurrentUserService _currentUserService;

    public TicketAttachmentService(IRepository<TicketAttachment> repository, IFileStorageService storageService, ICurrentTenantService currentTenantService, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _storageService = storageService;
        _currentTenantService = currentTenantService;
        _currentUserService = currentUserService;
    }

    public async Task<TicketAttachmentDto> UploadAsync(Guid ticketId, Stream fileStream, string fileName, string contentType)
    {
        var blobUrl = await _storageService.UploadAsync(fileStream, $"{Guid.NewGuid()}_{fileName}",contentType,"ticket-attachments");
        TicketAttachment newAttachment = new TicketAttachment(ticketId, fileName,blobUrl,_currentUserService.UserId,_currentTenantService.TenantId);
        await _repository.Add(newAttachment);
        TicketAttachmentDto dto = new TicketAttachmentDto(newAttachment.Id,newAttachment.TicketId,newAttachment.FileName, newAttachment.BlobUrl,newAttachment.UploadedById, newAttachment.UploadedAt);
        return dto;
    }

    public async Task<List<TicketAttachmentDto>> GetByTicketAsync(Guid ticketId)
    {
        var attachments = await _repository.GetByCondition(a=>a.TicketId==ticketId);
        return attachments.Select(a=> new TicketAttachmentDto(a.Id,a.TicketId,a.FileName, a.BlobUrl,a.UploadedById, a.UploadedAt)).ToList();
    }
}