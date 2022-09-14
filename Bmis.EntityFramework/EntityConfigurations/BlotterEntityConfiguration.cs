using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bmis.EntityFramework.EntityConfigurations;

public class BlotterEntityConfiguration : IEntityTypeConfiguration<Blotter>
{
    public void Configure(EntityTypeBuilder<Blotter> builder)
    {
        builder.ToTable("Blotters", ApplicationDbContext.DefaultSchema);

        builder.HasKey(r => r.Id);

        builder.Property(x => x.Complainant)
            .HasMaxLength(256);

        builder.Property(x => x.Respondent)
            .HasMaxLength(256);

        builder.Property(x => x.BlotterType)
            .HasMaxLength(16);

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder
            .HasOne(x => x.Barangay)
            .WithMany(x => x.Blotters)
            .OnDelete(DeleteBehavior.NoAction);
    }

}
