using HotelOpt.Domain.Entities;
using HoteOpt.Application.DTOs;
using HoteOpt.Application.Interfaces;

namespace HoteOpt.Application.Services;

public class PropertyService:IPropertyService
{
    private readonly IRepository<Property> _repository;
    private readonly ICurrentTenantService _currentTenantService;

    public PropertyService(IRepository<Property> repository, ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }
    public async Task<bool> AddProperty(CreatePropertyDto dto)
    {
        Property newProperty = new Property(dto.Name,dto.ContactEmail,dto.PhoneNumber,dto.StarRating, dto.Address, _currentTenantService.TenantId);
        var result = await _repository.Add(newProperty);
        return result;
    }

    public async Task UpdateProperty(UpdatePropertyDto dto)
    {
        Property property = await _repository.GetById(dto.Id);
        property.Update(dto.Name,dto.ContactEmail,dto.PhoneNumber,dto.StarRating,dto.Address);
        await _repository.Update(property);
    }

    public async Task<List<PropertyDto>> GetAllProperty()
    {
        List<Property> response = await _repository.GetAll();
        List<PropertyDto> list = response.Select(p => new PropertyDto(p.Id,p.Name,p.ContactEmail,p.PhoneNumber,p.Address, p.StarRating, p.TenantId)).ToList();
        return list;
    }

    public async Task<PropertyDto> GetPropertyById(Guid id)
    {
        Property propertyEntity = await _repository.GetById(id);
        PropertyDto dto = new PropertyDto(propertyEntity.Id, propertyEntity.Name, propertyEntity.ContactEmail, 
            propertyEntity.PhoneNumber, propertyEntity.Address, propertyEntity.StarRating, propertyEntity.TenantId);
        return dto;
    }

    public async Task<bool> DeleteProperty(Guid id)
    {
        var result = await _repository.Delete(id);
        return result;
    }
}