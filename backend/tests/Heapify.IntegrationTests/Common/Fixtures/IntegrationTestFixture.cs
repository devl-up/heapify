namespace Heapify.IntegrationTests.Common.Fixtures;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    public DatabaseFixture Database { get; private set; } = null!;
    public CustomWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Database = new DatabaseFixture();
        await Database.InitializeAsync();

        Factory = new CustomWebApplicationFactory(this);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Database.DisposeAsync();
    }
}