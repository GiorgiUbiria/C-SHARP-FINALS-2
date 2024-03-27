using Finals.Enums;

namespace Finals.Dtos;

public class UserStatusDto
{
    public Role Role { get; set; }
    
    public bool isBlocked { get; set; }
    
    public string Email { get; set; }
    
    public bool Action { get; set; }
}