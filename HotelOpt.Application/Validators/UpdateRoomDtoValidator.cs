using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Validators;

public class UpdateRoomDtoValidator:AbstractValidator<UpdateRoomDto>
{
    public UpdateRoomDtoValidator()
    {
        RuleFor(x => x.RoomNumber).MaximumLength(10).When(x => x.RoomNumber != null);
        RuleFor(x => x.Description).MaximumLength(500).When(x=>x.Description != null);
        RuleFor(x => x.Type).IsInEnum().When(x => x.Type != null);
        RuleFor(x => x.PropertyId).NotEmpty().When(x => x.PropertyId != null);
    }
}