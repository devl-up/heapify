using Heapify.IntegrationTests.Common.Collections;
using Heapify.IntegrationTests.Common.DTO;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Heapify.Presentation.Auth.DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heapify.IntegrationTests.Common;

[Collection(IntegrationTestCollection.Name)]
public abstract class IntegrationTestBase(IntegrationTestFixture fixture) : IAsyncLifetime
{
    protected DatabaseFixture Database => fixture.Database;
    protected CustomWebApplicationFactory Factory => fixture.Factory;


    public async Task InitializeAsync()
    {
        await fixture.Database.ResetAsync();
        await SeedUser();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected HttpClient CreateClient()
    {
        return fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            HandleCookies = true
        });
    }

    protected async Task SeedUser(
        string username = TestUser.Username,
        string email = TestUser.Email,
        string password = TestUser.Password
    )
    {
        using var client = await CreateClient().WithAntiforgeryAsync();

        var request = new RegisterRequest
        {
            Username = username,
            Email = email,
            Password = password
        };

        var response = await client.PostAsJsonAsync("/api/v1/auth/register", request);
        response.EnsureSuccessStatusCode();
    }
}