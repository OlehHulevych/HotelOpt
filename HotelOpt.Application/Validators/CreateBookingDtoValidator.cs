using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.PropertyId).NotEmpty();
        RuleFor(x => x.PrimaryGuestId).NotEmpty();
        RuleFor(x => x.CheckInDate).NotEmpty();
        RuleFor(x => x.CheckOutDate).NotEmpty()
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out must be after check-in");
    }
}
