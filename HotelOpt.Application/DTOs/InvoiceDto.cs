using HotelOpt.Domain.Enums;

namespace HotelOpt.Application.DTOs;

public record InvoiceDto(
    Guid Id,
    Guid BookingId,
    Guid RoomId,
    string GuestName,
    DateTimeOffset CheckInDate,
    DateTimeOffset CheckOutDate,
    int Nights,
    decimal PricePerNight,
    decimal TotalAmount,
    DateTimeOffset IssuedAt,
    InvoiceStatus Status
);
