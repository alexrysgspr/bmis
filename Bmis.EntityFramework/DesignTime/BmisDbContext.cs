using Bmis.EntityFramework.Entities;
using Bmis.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Bmis.EntityFramework.DesignTime;

public class BmisDbContext : DbContext
{
    public const string DefaultSchema = "bmis";
    public BmisDbContext(DbContextOptions<BmisDbContext> options) : base(options)
    {
    }

    public DbSet<Resident> Residents { get; set; }
    public DbSet<Blotter> Blotters { get; set; }

    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfiguration(new AddressEntityConfiguration())
            .ApplyConfiguration(new BlotterEntityConfiguration())
            .ApplyConfiguration(new ResidentEntityConfiguration());

        base.OnModelCreating(builder);
    }
}