using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Common;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        ApplicationDbContext context = CreateDbContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    protected ISender GetSender()
    {
        ISender sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        return sender;
    }

    protected ApplicationDbContext CreateDbContext()
    {
        ApplicationDbContext context = IntegrationTestDatabase.Create();

        return context;
    }
}
