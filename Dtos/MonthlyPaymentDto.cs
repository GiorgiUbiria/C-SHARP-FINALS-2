namespace Finals.Dtos;

public class MonthlyPaymentDto
{
    public required int Id { get; set; }
    
    public required decimal InitialAmount { get; set; } = 300;
    
    public required decimal AmountLeft { get; set; } = 300;

    public required decimal MonthlyDue { get; set; } = 300;
}