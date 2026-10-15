using System.Net;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.DTO;
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

        var loginRequest = new LoginRequest { Email = TestUser.Email, Password = TestUser.Password };

        // Act
        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

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

        var request = new LoginRequest { Email = "invalid@heapify.dev", Password = "Password123" };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}