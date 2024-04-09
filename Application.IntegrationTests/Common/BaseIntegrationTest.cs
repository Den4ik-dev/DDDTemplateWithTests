using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Common;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory>
{
    protected readonly ISender Sender;

    protected BaseIntegrationTest(IntegrationTestWebApplicationFactory factory)
    {
        IServiceScope scope = factory.Services.CreateScope();

        Sender = scope.ServiceProvider.GetRequiredService<ISender>();

        IntegrationTestDatabase.ClearDatabase();
    }

    protected ApplicationDbContext CreateDbContext()
    {
        ApplicationDbContext context = IntegrationTestDatabase.Create();

        return context;
    }
}
