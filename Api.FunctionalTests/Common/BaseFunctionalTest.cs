using Infrastructure.Data;

namespace Api.FunctionalTests.Common;

public class BaseFunctionalTest : IClassFixture<FunctionalWebApplicationFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalWebApplicationFactory factory)
    {
        HttpClient = factory.CreateClient();

        FunctionalTestDatabase.ClearDatabase();
    }

    protected ApplicationDbContext CreateDbContext()
    {
        ApplicationDbContext context = FunctionalTestDatabase.Create();

        return context;
    }
}
