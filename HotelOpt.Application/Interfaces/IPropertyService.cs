using HotelOpt.Application.Pagination;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IPropertyService
{
    public Task<bool> AddProperty(CreatePropertyDto dto);
    public Task UpdateProperty(UpdatePropertyDto dto);
    public Task<PaginatedResult<PropertyDto>> GetAllProperty(int pageSize, int currentPage);
    public Task<PropertyDto> GetPropertyById(Guid id);
    public Task<bool> DeleteProperty(Guid id);
}