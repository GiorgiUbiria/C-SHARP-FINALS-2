using Finals.Enums;

namespace Finals.Dtos;

public class FastLoanDto
{
        public required int Id { get; set; }
        
        public required int RequestedAmount { get; set; } = 300;
    
        public required decimal FinalAmount { get; set; } = 300 + (300 * ((decimal)LoanPeriod.OneYear / 100));
    
        public required LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;

        public required LoanType LoanType { get; set; } = LoanType.FAST;

        public required Currency LoanCurrency { get; set; } = Currency.GEL;

        public required LoanStatus LoanStatus { get; set; } = LoanStatus.PENDING;
}