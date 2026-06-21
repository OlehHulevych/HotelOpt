using FluentValidation;
using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Validators;

public class RegistrationDtoValidator:AbstractValidator<RegistrationDto>
{
    public RegistrationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).Matches("^[^0-9]*$").WithMessage("Name shouldn't contain any numbers");
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(20).Matches("^[^0-9]*$").WithMessage("Name shouldn't contain any numbers");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).Matches("[A-Z]").WithMessage("Should contain one uppercase letter").Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("Password must contain at least one special sign");
    }
}