using Bmis.EntityFramework.Entities;
using Bmis.EntityFramework.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bmis.EntityFramework.DesignTime
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public const string DefaultSchema = "bmis";
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Blotter> Blotters { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Barangay> Barangays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .ApplyConfiguration(new BarangayEntityConfiguration())
                .ApplyConfiguration(new IdentityRoleEntityConfiguration())
                .ApplyConfiguration(new ResidentEntityConfiguration())
                .ApplyConfiguration(new AddressEntityConfiguration())
                .ApplyConfiguration(new BlotterEntityConfiguration());

            base.OnModelCreating(builder);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entity in entities)
            {
                if (entity.Entity is not ISoftDeletableEntity softDeletedEntity) continue;
                entity.State = EntityState.Modified;
                    
                softDeletedEntity.IsDeleted = true;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
