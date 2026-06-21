using FluentValidation;
using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Validators;

public class UpdatePropertyDtoValidator:AbstractValidator<UpdatePropertyDto>
{
    public UpdatePropertyDtoValidator()
    {
        RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.ContactEmail).EmailAddress().NotEmpty();
        RuleFor(x => x.PhoneNumber).Matches("^\\+?[0-9]{7,15}$")
            .WithMessage("Number should have at least 7 to 15 digits");
        RuleFor(x => x.StarRating).InclusiveBetween(1, 5).NotEmpty();
    }
}