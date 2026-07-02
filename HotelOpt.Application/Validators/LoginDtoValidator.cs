using FluentValidation;
using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Validators;

public class LoginDtoValidator:AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Should not be empty").EmailAddress().WithMessage("Invalid email format");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Should not be empty");
    }
}