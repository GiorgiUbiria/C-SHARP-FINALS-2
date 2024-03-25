using Finals.Enums;

namespace Finals.Dtos;

public class CarLoanRequestDto
{
    public int CarId { get; set; } = 0;

    public LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;
}