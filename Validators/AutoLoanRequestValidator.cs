using Finals.Dtos;
using FluentValidation;

namespace Finals.Validators;

public class AutoLoanRequestDtoValidator : AbstractValidator<AutoLoanRequestDto>
{
    public AutoLoanRequestDtoValidator()
    {
        RuleFor(x => x.Model)
            .NotNull().WithMessage("Car model should be provided");
        RuleFor(x => x.LoanPeriod)
            .IsInEnum().WithMessage("Invalid loan period");
    }
}