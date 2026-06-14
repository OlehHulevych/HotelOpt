using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class Property:BaseEntity
{
    public string Name { get; private set; }
    public string ContactEmail { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public int StarRating { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }

    private Property()
    {
        Name = null!;
        ContactEmail = null!;
        PhoneNumber = null!;
        Address = null!;
        StarRating = 1;
        Tenant = null!;
    }

    public Property(string name, string contactEmail, string phoneNumber, int starRating, string address, Guid tenantId)
    {
        Name = name;
        ContactEmail = contactEmail;
        PhoneNumber = phoneNumber;
        StarRating = starRating;
        Address = address;
        TenantId = tenantId;
    }
    
}