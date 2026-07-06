using HotelOpt.Application.DTOs;
using HotelOpt.Application.Interfaces;
using HotelOpt.Application.Pagination;
using HotelOpt.Domain.Entities;
using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.Services;

public class BookingService:IBookingService
{
    private readonly IBookingRepository _repository;
    private readonly IRepository<Guest> _guestRepository;
    private readonly ICurrentTenantService _currentTenantService;

    public BookingService(IBookingRepository repository, ICurrentTenantService currentTenantService, IRepository<Guest> guestRepository)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _guestRepository = guestRepository;
    }
    public async Task<BookingResponseDto> CreateAsync(CreateBookingDto dto)
    {
        Guest leaderGuest = await _guestRepository.GetById(dto.PrimaryGuestId);
        var occupiedBooking = await _repository.GetByCondition(b=>b.RoomId==dto.RoomId && b.PropertyId == dto.PropertyId && dto.CheckInDate < b.CheckOutDate && dto.CheckOutDate > b.CheckInDate && b.Status!=BookingStatus.Cancelled && b.Status!=BookingStatus.CheckedOut);
        if (occupiedBooking.Any()) throw new Exception("This time is occupied");
        Booking booking = new Booking(dto.RoomId,dto.PropertyId,_currentTenantService.TenantId,dto.CheckInDate, dto.CheckOutDate, leaderGuest);
        await _repository.Add(booking);
        return new BookingResponseDto(booking.Id,booking.RoomId,booking.PropertyId,booking.CheckInDate,booking.CheckOutDate,booking.Status.ToString(), booking.Guests.Select(g=>$"{g.FirstName} {g.LastName}").ToList());

    }

    public async Task CheckInAsync(Guid bookingId)
    {
        Booking? booking = await _repository.GetById(bookingId);
        if (booking == null) throw new Exception("The booking is not found");
        booking.CheckIn();
        await _repository.Update(booking);
    }

    public async  Task CheckOutAsync(Guid bookingId)
    {
        Booking? booking = await _repository.GetById(bookingId);
        if (booking == null) throw new Exception("The booking is not found");
        booking.CheckOut();
        await _repository.Update(booking);
    }

    public async Task CancelAsync(Guid bookingId)
    {
        Booking? booking = await _repository.GetById(bookingId);
        if (booking == null) throw new Exception("The booking is not found");
        booking.Cancel();
        await _repository.Update(booking);
    }

    public async Task AddGuestAsync(Guid bookingId, Guid guestId)
    {
        Booking? booking = await _repository.GetById(bookingId);
        if (booking == null) throw new Exception($"The booking {bookingId} was not found");
        Guest? guest = await _guestRepository.GetById(guestId);
        if(guest==null) throw new Exception($"The Guest {guestId} was not found");
        booking.AddGuest(guest);
        await _repository.Update(booking);
        
    }

    public async Task<PaginatedResult<BookingResponseDto>> GetByPropertyAsync(Guid propertyId, int page, int pageSize)
    {
        (List<Booking> query, int total) = await _repository.GetByPropertyPaginated(propertyId,page, pageSize);
        List<BookingResponseDto> list = query.Select(b => new BookingResponseDto(b.Id,b.RoomId,b.PropertyId,b.CheckInDate, b.CheckOutDate,b.Status.ToString(), b.Guests.Select(g=>$"{g.FirstName} {g.LastName}").ToList())).ToList();
        return new PaginatedResult<BookingResponseDto>(list, total, pageSize, page);
    }
}
