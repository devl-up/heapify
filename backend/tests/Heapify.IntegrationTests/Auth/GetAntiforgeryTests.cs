using System.Net;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Fixtures;

namespace Heapify.IntegrationTests.Auth;

public sealed class GetAntiforgeryTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Get_AntiforgeryToken_ReturnsToken()
    {
        // Arrange
        using var client = CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/auth/antiforgery");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var cookie = response.Headers.GetValues("Set-Cookie")
            .First(c => c.StartsWith("XSRF-TOKEN="));

        var token = Uri.UnescapeDataString(cookie.Split(';')[0]["XSRF-TOKEN=".Length..]);
        Assert.NotNull(token);
    }
}