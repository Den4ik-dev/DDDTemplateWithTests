using Infrastructure.Data;

namespace Api.FunctionalTests.Common;

public class BaseFunctionalTest : IClassFixture<FunctionalWebApplicationFactory>
{
    protected HttpClient Client;

    protected BaseFunctionalTest(FunctionalWebApplicationFactory factory)
    {
        Client = factory.CreateClient();

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        ApplicationDbContext context = CreateDbContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    protected ApplicationDbContext CreateDbContext()
    {
        ApplicationDbContext context = FunctionalTestDatabase.Create();

        return context;
    }
}
