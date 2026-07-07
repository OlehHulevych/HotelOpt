using System.Collections.Specialized;
using FluentValidation;
using HotelOpt.Domain.Entities;
using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Services;

public class PropertyService:IPropertyService
{
    private readonly IRepository<Property> _repository;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IValidator<UpdatePropertyDto> _updateValidator;
    private readonly IValidator<CreatePropertyDto> _createValidator;

    public PropertyService(IRepository<Property> repository, ICurrentTenantService currentTenantService, IValidator<UpdatePropertyDto> updateValidator, IValidator<CreatePropertyDto> createValidator, IRepository<Tenant> tenantRepository)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _tenantRepository = tenantRepository;
    }
    public async Task<bool> AddProperty(CreatePropertyDto dto)
    {
        var validResult = await _createValidator.ValidateAsync(dto);
        if (!validResult.IsValid) throw new ValidationException(validResult.Errors);
        Tenant? tenant = await _tenantRepository.GetById(_currentTenantService.TenantId);
        if (tenant == null) throw new Exception("Tenant is not found");
        var properties = await _repository.GetByCondition(p=>p.TenantId==tenant.Id);
        int count = properties.Count;
        if (tenant.SubscriptionPlan == SubscriptionPlan.Trial)
        {
            if (count >= 1) throw new Exception("Limit of properties is reached.Please upgrade your subscription to create new one");
        }

        if (tenant.SubscriptionPlan == SubscriptionPlan.Basic)
        {
            if(count>=3) throw new Exception("Limit of properties is reached.Please upgrade your subscription to create new one");
        }
        
        
        Property newProperty = new Property(dto.Name,dto.ContactEmail,dto.PhoneNumber,dto.StarRating, dto.Address, _currentTenantService.TenantId);
        var result = await _repository.Add(newProperty);
        return result;
    }

    public async Task UpdateProperty(UpdatePropertyDto dto)
    {
        var validResult = await _updateValidator.ValidateAsync(dto);
        if (!validResult.IsValid) throw new ValidationException(validResult.Errors);
        Property property = await _repository.GetById(dto.Id);
        property.Update(dto.Name,dto.ContactEmail,dto.PhoneNumber,dto.StarRating,dto.Address);
        await _repository.Update(property);
    }

    public async Task<PaginatedResult<PropertyDto>> GetAllProperty(int pageSize, int currentPage)
    {
        (List<Property> response, int totalCount) = await _repository.GetAllPaginated(currentPage, pageSize);
        List<PropertyDto> list = response.Select(p => new PropertyDto(p.Id,p.Name,p.ContactEmail,p.PhoneNumber,p.Address, p.StarRating, p.TenantId)).ToList();
        return new PaginatedResult<PropertyDto>(list,totalCount,pageSize,currentPage);
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