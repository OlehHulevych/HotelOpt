namespace HoteOpt.Application.Interfaces;

public interface ICurrentTenantService
{
    Guid TenantId { get; }
}