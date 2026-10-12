using Heapify.Domain.Users.Entities;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Heapify.Presentation.Auth.DTO;
using Microsoft.AspNetCore.Identity;

namespace Heapify.IntegrationTests.Auth;

public sealed class RegisterTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Post_Register_ReturnsOk_WhenValidRequest()
    {
        // Arrange
        using var client = await CreateClient()
            .WithAntiforgeryAsync();

        const string username = "test";
        const string email = "test@heapify.dev";
        const string password = "Password123";

        var registerDto = new RegisterDto { Username = username, Email = email, Password = password };

        // Act
        var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/register", registerDto);

        // Assert
        registerResponse.EnsureSuccessStatusCode();

        await using var serviceScope = Factory.Services.CreateAsyncScope();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var user = await userManager.FindByEmailAsync(email);
        Assert.NotNull(user);

        var passwordValid = await userManager.CheckPasswordAsync(user, password);
        Assert.True(passwordValid);
    }
}