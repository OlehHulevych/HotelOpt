using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Validators;

public class UpdateGuestDtoValidator : AbstractValidator<UpdateGuestDto>
{
    public UpdateGuestDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).MaximumLength(100).When(x => x.FirstName != null);
        RuleFor(x => x.LastName).MaximumLength(100).When(x => x.LastName != null);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(200).When(x => x.Email != null);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone != null);
    }
}
