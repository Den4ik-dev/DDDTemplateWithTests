using Infrastructure.Data;

namespace Api.FunctionalTests.Common;

public class BaseFunctionalTest : IClassFixture<FunctionalWebApplicationFactory>
{
    protected readonly HttpClient Client;

    protected BaseFunctionalTest(FunctionalWebApplicationFactory factory)
    {
        Client = factory.CreateClient();

        FunctionalTestDatabase.ClearDatabase();
    }

    protected ApplicationDbContext CreateDbContext()
    {
        ApplicationDbContext context = FunctionalTestDatabase.Create();

        return context;
    }
}
