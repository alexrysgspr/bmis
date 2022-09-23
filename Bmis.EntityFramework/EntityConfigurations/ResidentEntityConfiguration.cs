using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bmis.EntityFramework.EntityConfigurations;

public class ResidentEntityConfiguration : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.ToTable("Residents", ApplicationDbContext.DefaultSchema);

        builder.HasKey(r => r.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.MiddleName)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.LastName)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.MiddleName)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.Extension)
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.CivilStatus)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.MinLimit);

        builder.Property(x => x.VoterStatus)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.MinLimit);

        builder.Property(x => x.Gender)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.MinLimit);

        builder.Property(x => x.ContactNo)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.MinLimit);

        builder.Property(x => x.Email)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder.Property(x => x.Disability)
            .HasConversion<string>()
            .HasMaxLength(PropertyLimits.DefaultLimit);

        builder
            .HasOne(x => x.Barangay)
            .WithMany(x => x.Residents)
            .HasForeignKey(x => x.BarangayId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(b => !b.IsDeleted);

    }

    public class PropertyLimits
    {
        public static int MinLimit = 16;
        public static int DefaultLimit = 32;
        public static int MaxLimit = 256;
    }
}
