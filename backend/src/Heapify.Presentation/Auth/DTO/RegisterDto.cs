namespace Heapify.Presentation.Auth.DTO;

public sealed class RegisterDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}