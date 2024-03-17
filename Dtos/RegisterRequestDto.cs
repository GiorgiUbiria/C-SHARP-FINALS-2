using System.ComponentModel.DataAnnotations;
using Finals.Enums;

namespace Finals.Dtos;

public class RegisterRequestDto
{
    [Required] public string? Email { get; set; } = string.Empty;

    [Required] public string? Username { get; set; } = string.Empty;

    [Required] public string? Password { get; set; } = string.Empty;

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    [Required] public int Age { get; set; } = 0;

    [Required] public int Salary { get; set; } = 0;
}