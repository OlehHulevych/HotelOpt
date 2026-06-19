using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IPropertyService
{
    public Task<bool> AddProperty(CreatePropertyDto dto);
    public Task UpdateProperty(UpdatePropertyDto dto);
    public Task<List<PropertyDto>> GetAllProperty();
    public Task<PropertyDto> GetPropertyById(Guid id);
    public Task<bool> DeleteProperty(Guid id);
}