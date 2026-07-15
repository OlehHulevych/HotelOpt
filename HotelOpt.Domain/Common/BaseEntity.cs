namespace HotelOpt.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public  DateTimeOffset UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public void Delete() => IsDeleted = true;
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}