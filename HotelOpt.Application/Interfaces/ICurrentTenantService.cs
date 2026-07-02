namespace HotelOpt.Application.Interfaces;

public interface ICurrentTenantService
{
    Guid TenantId { get; }
}