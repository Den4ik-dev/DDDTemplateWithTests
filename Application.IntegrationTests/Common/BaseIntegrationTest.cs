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
        ApplicationDbContext context = IntegrationTestDatabase.Create();

        return context;
    }
}
