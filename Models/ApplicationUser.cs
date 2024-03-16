using Finals.Enums;
using Microsoft.AspNetCore.Identity;

namespace Finals.Models;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}