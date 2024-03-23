using Finals.Dtos;
using FluentValidation;

namespace Finals.Validators;

public class InstallmentLoanRequestDtoValidator : AbstractValidator<InstallmentLoanRequestDto>
{
    public InstallmentLoanRequestDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("ProductId is required.");
        RuleFor(x => x.LoanPeriod)
            .NotEmpty().IsInEnum().WithMessage("Invalid loan period");
        RuleFor(x => x.LoanType)
            .NotEmpty().IsInEnum().WithMessage("Invalid loan type");
        RuleFor(x => x.LoanCurrency)
            .NotEmpty().IsInEnum().WithMessage("Invalid currency");
    } 
}