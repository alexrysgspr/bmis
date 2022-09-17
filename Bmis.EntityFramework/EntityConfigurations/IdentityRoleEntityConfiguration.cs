using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bmis.EntityFramework.EntityConfigurations;
public class IdentityRoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(new List<IdentityRole>()
        {
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            },
        });
    }
}
