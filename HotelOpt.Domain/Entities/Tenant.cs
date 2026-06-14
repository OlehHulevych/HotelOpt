using HotelOpt.Domain.Common;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Domain.Entities;


public class Tenant:BaseEntity
{
    public string Name { get; private set; } 
    public string ContactEmail { get; private set; }
    public TenantStatus Status { get; private set; } = TenantStatus.Pending;

    private Tenant()
    {
        Name = null!;
        ContactEmail = null!;
    }

    public  Tenant(string name, string contactEmail)
    {
        Name = name;
        ContactEmail = contactEmail;
    }
    

}