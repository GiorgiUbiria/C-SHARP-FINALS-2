using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finals.Enums;

namespace Finals.Models;

public class Loan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int Amount { get; set; }
    
    public DateTime LoanPeriod { get; set; }
    
    public LoanType LoanType { get; set; }
    
    public Currency LoanCurrency { get; set; }
    
    public string ApplicationUserId { get; set; }
    
    public ApplicationUser ApplicationUser { get; set; }
}