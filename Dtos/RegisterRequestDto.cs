using System.ComponentModel.DataAnnotations;
using Finals.Enums;

namespace Finals.Dtos;

public class RegisterRequestDto
{
    public required string? Email { get; set; } = string.Empty;

    public required string? Username { get; set; } = string.Empty;

    public required string? Password { get; set; } = string.Empty;

    public required string FirstName { get; set; } = string.Empty;

    public required string LastName { get; set; } = string.Empty;

    public required int Age { get; set; } = 0;

    public required int Salary { get; set; } = 0;
}