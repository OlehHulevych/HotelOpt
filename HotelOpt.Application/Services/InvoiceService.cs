using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Application.Services;

public class InvoiceService:IInvoiceService
{
    private readonly IRepository<Booking> _bookingRepository;
    private readonly IRepository<Invoice> _invoiceRepository;
    

    public InvoiceService(IRepository<Booking> bookingRepository, IRepository<Invoice> invoiceRepository)
    {
        _bookingRepository = bookingRepository;
        _invoiceRepository = invoiceRepository;

    }
    public async Task GenerateAsync(Guid bookingId)
    {
        Booking? booking = await _bookingRepository.GetByIdWithIncludes(bookingId,b=>b.Room, b=>b.Guests);
        if (booking == null) throw new Exception("Booking was not found");
        List<Guest> bookingGuests = booking!.Guests.ToList();
        Invoice newInvoice = new Invoice(booking.Id, booking.RoomId,bookingGuests[0].FirstName + " " + bookingGuests[0].LastName,
            booking.CheckInDate, booking.CheckOutDate,booking.TenantId, booking.Room.PricePerNight );
        await _invoiceRepository.Add(newInvoice);

    }

    public async Task<InvoiceDto> GetByBookingAsync(Guid bookingId)
    {
        Invoice? invoice = await _invoiceRepository.GetSingleByCondition(i=>i.BookingId == bookingId);
        if (invoice == null) throw new Exception("Invoice was not found");
        return new InvoiceDto(invoice.Id, invoice.BookingId,invoice.RoomId,invoice.GuestName,invoice.CheckInDate,
            invoice.CheckOutDate,invoice.Nights,invoice.PricePerNight,invoice.TotalAmount,invoice.IssuedAt,invoice.Status);
    }
}