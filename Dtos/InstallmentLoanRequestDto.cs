using Finals.Enums;

namespace Finals.Dtos;

public class InstallmentLoanRequestDto
{
    public int ProductId { get; set; } = 0;

    public LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;
}