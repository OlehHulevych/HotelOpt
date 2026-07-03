using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class GuestService : IGuestService
{
    private readonly IRepository<Guest> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public GuestService(IRepository<Guest> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }

    public async Task<bool> AddGuest(CreateGuestDto dto)
    {
        var guest = new Guest(dto.FirstName, dto.LastName, dto.PassportNumber, _currentTenantService.TenantId, dto.Email, dto.Phone);
        return await _repository.Add(guest);
    }

    public async Task UpdateGuest(UpdateGuestDto dto)
    {
        var guest = await _repository.GetById(dto.Id);
        guest.Update(dto.FirstName, dto.LastName, dto.Email, dto.Phone);
        await _repository.Update(guest);
    }

    public async Task<GuestDto> GetById(Guid id)
    {
        var guest = await _repository.GetById(id);
        return new GuestDto(guest.Id, guest.FirstName, guest.LastName, guest.Email, guest.Phone, guest.PassportNumber);
    }

    public async Task<PaginatedResult<GuestDto>> GetAll(int currentPage, int pageSize)
    {
        var (guests, totalCount) = await _repository.GetAllPaginated(currentPage, pageSize);
        var list = guests.Select(g => new GuestDto(g.Id, g.FirstName, g.LastName, g.Email, g.Phone, g.PassportNumber)).ToList();
        return new PaginatedResult<GuestDto>(list, totalCount, pageSize, currentPage);
    }

    public async Task<bool> DeleteGuest(Guid id)
    {
        return await _repository.Delete(id);
    }
}
