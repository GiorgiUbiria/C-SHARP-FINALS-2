using Finals.Enums;
using Finals.Models;

public class LoanDto
{
    public required int Id { get; set; }
    
    public required decimal RequestedAmount { get; set; } = 300;

    public required decimal FinalAmount { get; set; } = 300;
    
    public required decimal AmountLeft { get; set; } = 300;
    
    public required LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;

    public required LoanType LoanType { get; set; } = LoanType.FAST;

    public required Currency LoanCurrency { get; set; } = Currency.GEL;

    public required LoanStatus LoanStatus { get; set; } = LoanStatus.PENDING;
    
    public required int ProductId { get; set; }
    
    public required Product Product { get; set; }
    
    public required int CarId { get; set; }
    
    public required Car Car { get; set; }
    
    public required string UserEmail { get; set; }
}