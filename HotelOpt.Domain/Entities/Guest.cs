using HotelOpt.Domain.Common;

namespace HotelOpt.Domain.Entities;

public class Guest:BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PassportNumber { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    private Guest ()
    {
        FirstName = null!;
        LastName = null!;
        PassportNumber = null!;
        Tenant = null!;
        Email = null!;
        Phone = null!;
    }

    public Guest(string firstName, string lastName, string passportNumber, Guid tenantId, string email, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        PassportNumber = passportNumber;
        TenantId = tenantId;
        Email = email;
        Phone = phone;
    }
    
    public void Update(string? firstName, string? lastName, string? email, string? phone)
    {
        FirstName = firstName ?? FirstName;
        LastName = lastName ?? LastName;
        Email = email ?? Email;
        Phone = phone ?? Phone;
    }
}