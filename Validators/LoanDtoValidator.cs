using FluentValidation;

namespace Finals.Validators;

public class LoanDtoValidator : AbstractValidator<LoanDto>
{
    public LoanDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

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