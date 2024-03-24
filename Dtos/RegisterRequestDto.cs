using System.ComponentModel.DataAnnotations;
using Finals.Enums;

namespace Finals.Dtos;

public class RegisterRequestDto
{
    public required string? Email { get; set; } = "test@test.com";

    public required string? Username { get; set; } = "testUser";

    public required string? Password { get; set; } = "testPassword";

    public required string FirstName { get; set; } = "testFirstName";

    public required string LastName { get; set; } = "testLastName";

    public required int Age { get; set; } = 18;

    public required int Salary { get; set; } = 600;
}