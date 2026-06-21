using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HoteOpt.Application.Validators;

public class CreateRoomDtoValidator:AbstractValidator<CreateRoomDto>
{
    public CreateRoomDtoValidator()
    {
        RuleFor(x => x.RoomNumber).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.PropertyId).NotEmpty();
    }
}