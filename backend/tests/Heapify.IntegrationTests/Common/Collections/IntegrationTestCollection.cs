using Heapify.IntegrationTests.Common.Fixtures;

namespace Heapify.IntegrationTests.Common.Collections;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
    public const string Name = "IntegrationTest";
}