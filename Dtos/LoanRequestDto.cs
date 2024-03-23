using Finals.Enums;

namespace Finals.Dtos;

public class LoanRequestDto
{
    public required int RequestedAmount { get; set; } = 300;
    
    public required LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;

    public required LoanType LoanType { get; set; } = LoanType.FAST;

    public required Currency LoanCurrency { get; set; } = Currency.GEL;
}