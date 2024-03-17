using Finals.Enums;
using Microsoft.AspNetCore.Identity;

namespace Finals.Models;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }

    public bool IsBlocked { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public int Age { get; set; }
    
    public int Salary { get; set; }
    
    public ICollection<Loan> Loans { get; set; }
}