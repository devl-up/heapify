using Heapify.IntegrationTests.Common.Fixtures;
using Heapify.Presentation;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heapify.IntegrationTests.Common;

public sealed class CustomWebApplicationFactory(IntegrationTestFixture fixture)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Postgres", fixture.Database.ConnectionString);
    }
}