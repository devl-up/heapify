using Heapify.IntegrationTests.Common.Collections;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heapify.IntegrationTests.Common;

[Collection(IntegrationTestCollection.Name)]
public abstract class IntegrationTestBase(IntegrationTestFixture fixture) : IAsyncLifetime
{
    protected DatabaseFixture Database => fixture.Database;

    public async Task InitializeAsync()
    {
        await fixture.Database.ResetAsync();
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
}