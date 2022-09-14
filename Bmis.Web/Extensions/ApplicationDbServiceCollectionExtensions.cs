using Bmis.EntityFramework.DesignTime;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Extensions;

public static class ApplicationDbServiceCollectionExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    await appContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        }

        return host;
    }
}
