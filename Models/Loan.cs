using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finals.Enums;

namespace Finals.Models;

public class Loan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int RequstedAmount { get; set; }
    
    public int FinalAmount { get; set; }
    
    public LoanPeriod LoanPeriod { get; set; }
    
    public LoanType LoanType { get; set; }
    
    public Currency LoanCurrency { get; set; }
    
    public LoanStatus LoanStatus { get; set; }
    
    public string ApplicationUserId { get; set; }
    
    public ApplicationUser ApplicationUser { get; set; }
    
    public int? ProductId { get; set; } 
    
    public Product? Product { get; set; }
}