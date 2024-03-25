using Finals.Enums;

namespace Finals.Dtos;

public class AutoLoanRequestDto
{
    public string Model { get; set; } = string.Empty;

    public LoanPeriod LoanPeriod { get; set; } = LoanPeriod.OneYear;
}