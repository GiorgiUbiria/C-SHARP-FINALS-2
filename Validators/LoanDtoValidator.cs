using Finals.Enums;
using FluentValidation;

namespace Finals.Validators;

public class LoanDtoValidator : AbstractValidator<LoanDto>
{
    public LoanDtoValidator()
    {
        RuleFor(x => x.RequestedAmount)
            .GreaterThanOrEqualTo(300).WithMessage("Requested Amount must be greater than 300");
        RuleFor(x => x.FinalAmount)
            .GreaterThanOrEqualTo(300 + (300 * ((decimal)LoanPeriod.HalfYear / 100))).WithMessage("Final Amount must be greater than 5% more than minimal requested amount");
        RuleFor(x => x.LoanPeriod)
            .IsInEnum().WithMessage("Invalid loan period");
        RuleFor(x => x.LoanType)
            .IsInEnum().WithMessage("Invalid loan type");
        RuleFor(x => x.LoanCurrency)
            .IsInEnum().WithMessage("Invalid currency");
        RuleFor(x => x.LoanStatus)
            .IsInEnum().WithMessage("Invalid loan status");
    }
}