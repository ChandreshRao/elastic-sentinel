using Xunit;

namespace ElasticSentinel.Tests.API;

[CollectionDefinition("API Tests")]
public class ApiTestCollection : ICollectionFixture<TestWebApplicationFactory>
{
    // This class has no code, and is never instantiated.
    // Its purpose is simply to be the place to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
}
