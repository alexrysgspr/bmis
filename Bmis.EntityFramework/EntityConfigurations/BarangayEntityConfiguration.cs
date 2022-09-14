using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bmis.EntityFramework.EntityConfigurations;

public class BarangayEntityConfiguration : IEntityTypeConfiguration<Barangay>
{
    public void Configure(EntityTypeBuilder<Barangay> builder)
    {
        builder.ToTable("Residents", ApplicationDbContext.DefaultSchema);

        builder.HasKey(r => r.Id);

        builder
            .Property(x => x.Host)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder
            .Property(x => x.Name)
            .HasMaxLength(PropertyLimits.MaxLimit);

        builder
            .Property(x => x.Logo)
            .HasMaxLength(PropertyLimits.MaxLimit);
    }

    public class PropertyLimits
    {
        public static int MinLimit = 16;
        public static int DefaultLimit = 32;
        public static int MaxLimit = 256;
    }
}
