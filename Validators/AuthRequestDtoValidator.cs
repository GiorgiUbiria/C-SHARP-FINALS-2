using Finals.Dtos;
using FluentValidation;

namespace Finals.Validators;


public class AuthRequestDtoValidator : AbstractValidator<AuthRequestDto>
{
    public AuthRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}