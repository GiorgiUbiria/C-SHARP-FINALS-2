namespace Finals.Dtos;

public class LoanDto
{
    public required int Id { get; set; }
    
    public required int? Amount { get; set; }
    
    public required DateTime? LoanPeriod { get; set; }
}