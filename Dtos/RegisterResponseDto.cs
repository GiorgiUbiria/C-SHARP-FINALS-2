using Microsoft.AspNetCore.Identity;

namespace Finals.Dtos;

public class RegisterResponseDto
{
    public IdentityResult User { get; set; }
    public string Message { get; set; }
}