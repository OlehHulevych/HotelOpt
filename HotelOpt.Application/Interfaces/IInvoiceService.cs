using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IInvoiceService
{
    Task GenerateAsync(Guid bookingId);
    Task<InvoiceDto> GetByBookingAsync(Guid bookingId);
}
