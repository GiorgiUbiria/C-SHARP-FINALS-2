namespace Finals.Dtos;

public class AuthRequestDto
{
    public required string Email { get; set; } = "test@test.com";

    public required string Password { get; set; } = string.Empty;
}