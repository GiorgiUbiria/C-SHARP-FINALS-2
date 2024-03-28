using Finals.Dtos;
using FluentValidation;

namespace Finals.Validators;

public class FastLoanRequestDtoValidator : AbstractValidator<FastLoanRequestDto>
{
    public FastLoanRequestDtoValidator()
    {
        RuleFor(x => x.RequestedAmount)
            .GreaterThanOrEqualTo(500).WithMessage("Requested Amount must be greater than 500");
        RuleFor(x => x.LoanPeriod)
            .IsInEnum().WithMessage("Invalid loan period");
        RuleFor(x => x.LoanType)
            .IsInEnum().WithMessage("Invalid loan type");
        RuleFor(x => x.LoanCurrency)
            .IsInEnum().WithMessage("Invalid currency");
    }
}