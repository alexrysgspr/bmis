using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bmis.EntityFramework.DesignTime;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets(typeof(ApplicationDbContext).Assembly)
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var connectionString = configuration.GetValue<string>("SqlConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string must be provided", nameof(connectionString));
        }

        builder
            .UseSqlServer(connectionString, x => { x.EnableRetryOnFailure(); });

        return new ApplicationDbContext(builder.Options);    
    }
}