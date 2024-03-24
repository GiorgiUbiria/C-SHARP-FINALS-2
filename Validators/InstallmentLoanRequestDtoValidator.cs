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
            .IsInEnum().WithMessage("Invalid loan period");
    } 
}