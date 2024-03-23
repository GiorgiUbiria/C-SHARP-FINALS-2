using Finals.Enums;

namespace Finals.Dtos;

public class InstallmentLoanRequestDto
{
    public required int ProductId { get; set; } = 0;

    public required LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;

    public required LoanType LoanType { get; set; } = LoanType.FAST;

    public required Currency LoanCurrency { get; set; } = Currency.GEL;
}