namespace Heapify.Presentation.Auth.DTO;

public sealed class LoginDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}