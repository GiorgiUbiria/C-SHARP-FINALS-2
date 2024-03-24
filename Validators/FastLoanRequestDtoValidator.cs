using Finals.Dtos;
using Finals.Enums;
using FluentValidation;

namespace Finals.Validators;

public class FastLoanRequestDtoValidator : AbstractValidator<FastLoanRequestDto>
{
    public FastLoanRequestDtoValidator()
    {
        RuleFor(x => x.RequestedAmount)
            .GreaterThanOrEqualTo(300).WithMessage("Requested Amount must be greater than 300");
        RuleFor(x => x.LoanPeriod)
            .IsInEnum().WithMessage("Invalid loan period");
        RuleFor(x => x.LoanType)
            .IsInEnum().WithMessage("Invalid loan type");
        RuleFor(x => x.LoanCurrency)
            .IsInEnum().WithMessage("Invalid currency");
    }
}