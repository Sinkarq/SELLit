namespace SELLit.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTests))]
public class SharedIntegrationTestFactory : ICollectionFixture<IntegrationTestFactory<Startup, ApplicationDbContext>>
{
}