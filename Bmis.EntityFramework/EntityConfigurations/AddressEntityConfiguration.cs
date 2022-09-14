using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bmis.EntityFramework.EntityConfigurations;

public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses", ApplicationDbContext.DefaultSchema);

        builder.HasKey(r => r.Id);

        builder.Property(x => x.Purok)
            .HasMaxLength(32);

        builder
            .HasMany(x => x.Residents)
            .WithOne(x => x.Address)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.AddressLine)
            .HasMaxLength(256);

        builder
            .HasOne(x => x.Barangay)
            .WithMany(x => x.Addresses)
            .OnDelete(DeleteBehavior.SetNull);
    }

}
