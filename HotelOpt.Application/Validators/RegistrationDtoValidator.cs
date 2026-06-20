using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using HoteOpt.Application.DTOs;

namespace HoteOpt.Application.Validators;

public class RegistrationDtoValidator:AbstractValidator<RegistrationDto>
{
    public RegistrationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).Matches("^[^0-9]*$");
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(20).Matches("^[^0-9]*$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).Matches("[A-Z]").Matches("[0-9]")
            .Matches("[!@#$%^&*(),.?\":{}|<>]");
    }
}