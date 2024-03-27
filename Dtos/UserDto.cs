using Finals.Enums;
using Finals.Models;

namespace Finals.Dtos;

public class UserDto
{
    public Role Role { get; set; }

    public bool IsBlocked { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public int Age { get; set; }
    
    public int Salary { get; set; }
    
    public ICollection<Loan> Loans { get; set; }
}