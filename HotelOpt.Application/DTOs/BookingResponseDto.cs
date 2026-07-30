namespace HotelOpt.Application.DTOs;

public record BookingResponseDto(
    Guid Id,
    Guid RoomId,
    string RoomNumber,
    Guid PropertyId,
    DateTimeOffset CheckInDate,
    DateTimeOffset CheckOutDate,
    string Status,
    List<string> GuestNames);
