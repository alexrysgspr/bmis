using Bmis.EntityFramework.DesignTime;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bmis.EntityFramework.Extensions
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddAppPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BmisDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
                //options.UseInMemoryDatabase("Bmis");
                options.UseExceptionProcessor();
            });

            return services;
        }
    }
}
