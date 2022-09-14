using Bmis.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bmis.EntityFramework.DesignTime
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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
            base.OnModelCreating(builder);
        }
    }
}
