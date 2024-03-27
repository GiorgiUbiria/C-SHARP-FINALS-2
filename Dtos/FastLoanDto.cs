using Finals.Enums;

namespace Finals.Dtos;

public class FastLoanDto
{
        public required int Id { get; set; }
        
        public required decimal RequestedAmount { get; set; } = 300;
    
        public required decimal FinalAmount { get; set; } = 300;

        public required decimal AmountLeft { get; set; } = 300;
    
        public required LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;

        public required LoanType LoanType { get; set; } = LoanType.FAST;

        public required Currency LoanCurrency { get; set; } = Currency.GEL;

        public required LoanStatus LoanStatus { get; set; } = LoanStatus.PENDING;
}