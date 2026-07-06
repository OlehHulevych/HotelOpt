using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Interfaces;

public interface IBookingRepository:IRepository<Booking>
{
    Task<(List<Booking> Items, int TotalCount)> GetByPropertyPaginated(Guid propertyId, int page, int pageSize);
}