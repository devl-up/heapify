using System.Net;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Heapify.Presentation.Auth.DTO;

namespace Heapify.IntegrationTests.Auth;

public sealed class GetMeTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Get_Me_ReturnsUserInfo_WhenAuthenticated()
    {
        // Arrange
        const string username = "username";
        using var client = await CreateClient().AuthenticateAsync(username);

        // Act
        var user = await client.GetFromJsonAsync<UserResponse>("/api/v1/auth/me");

        // Assert
        var expectedUser = new UserResponse { Name = username };
        Assert.NotNull(user);
        Assert.Equivalent(expectedUser, user);
    }

    [Fact]
    public async Task Get_Me_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        using var client = await CreateClient().WithAntiforgeryAsync();

        // Act
        var response = await client.GetAsync("/api/v1/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}