using Finals.Enums;

public class LoanDto
{
    public required int Amount { get; set; } = 0;

    public required DateTime LoanPeriod { get; set; } = DateTime.Now;

    public required LoanType LoanType { get; set; } = LoanType.FAST;

    public required Currency LoanCurrency { get; set; } = Currency.GEL;

    public required LoanStatus LoanStatus { get; set; } = LoanStatus.PENDING;
}