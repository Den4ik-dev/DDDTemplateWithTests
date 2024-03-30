using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.IntegrationTests.Common;

public static class IntegrationTestDatabase
{
    private const string Path = "appsettings.json";

    private static string _connectionString = string.Empty;

    public static string ConnectionString
    {
        get
        {
            if (_connectionString == string.Empty)
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddJsonFile(Path);

                IConfigurationRoot config = configBuilder.Build();

                _connectionString = config.GetConnectionString("Testing")!;
            }

            return _connectionString;
        }
    }

    public static ApplicationDbContext Create()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        DbContextOptions<ApplicationDbContext> options = optionsBuilder
            .UseSqlServer(ConnectionString)
            .Options;

        var context = new ApplicationDbContext(options);

        return context;
    }
}
