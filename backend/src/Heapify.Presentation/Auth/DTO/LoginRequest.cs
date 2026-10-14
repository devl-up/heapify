namespace Heapify.Presentation.Auth.DTO;

public sealed class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}