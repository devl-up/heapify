using System.Net;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Heapify.Presentation.Auth.DTO;

namespace Heapify.IntegrationTests.Auth;

public sealed class LoginTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Post_Login_ReturnsOk_WhenValidRequest()
    {
        // Arrange
        using var client = await CreateClient()
            .WithAntiforgeryAsync();

        const string username = "test";
        const string email = "test@heapify.dev";
        const string password = "Password123";

        var registerDto = new RegisterDto { Username = username, Email = email, Password = password };
        var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/register", registerDto);
        registerResponse.EnsureSuccessStatusCode();

        var loginDto = new LoginDto { Email = email, Password = password };

        // Act
        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert
        Assert.True(loginResponse.Headers.TryGetValues("Set-Cookie", out var cookies));
        var authCookie = cookies.SingleOrDefault(c => c.StartsWith(".AspNetCore.Identity.Application="));

        Assert.NotNull(authCookie);
        Assert.Contains("httponly", authCookie, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Post_Login_ReturnsUnauthorized_WhenInvalidRequest()
    {
        // Arrange
        using var client = await CreateClient()
            .WithAntiforgeryAsync();

        var dto = new LoginDto { Email = "invalid@heapify.dev", Password = "Password123" };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}