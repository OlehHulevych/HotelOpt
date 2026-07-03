using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Validators;

public class CreateGuestDtoValidator : AbstractValidator<CreateGuestDto>
{
    public CreateGuestDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PassportNumber).NotEmpty().MaximumLength(50);
    }
}
