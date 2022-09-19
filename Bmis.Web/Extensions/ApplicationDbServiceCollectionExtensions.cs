using System.Security.Claims;
using System.Text.Json;
using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Extensions;

public static class ApplicationDbServiceCollectionExtensions
{
    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        await using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (appContext.Database.IsSqlServer())
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

        await host.Seed();
    }

    public static async Task Seed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await CreateDefaultUserAndRoleForApplication(dbContext, userManager, roleManager);
    }

    private static async Task CreateDefaultUserAndRoleForApplication(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
    {
        var rolesToCreate = new List<string>
            {
                Roles.SuperAdmin,
                Roles.Admin
            };

        var currentRoles = await roleManager.Roles.ToListAsync();
        foreach (var role in Roles.All)
        {
            if (!currentRoles.Any(x => role.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                rolesToCreate.Add(role);
            }
        }

        await CreateRoles(roleManager, rolesToCreate);

        if (!userManager.Users.Any())
        {
            await SetupNewUser(dbContext, userManager, "admin@bmis.com", "Pass123!", Roles.All);
        }
    }

    private static async Task SetupNewUser(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string[] roles)
    {
        var barangay = await CreateBarangay(dbContext);
        await CreateResidents(dbContext, barangay);
        await CreateBlotters(dbContext, barangay);
        var user = await CreateUser(userManager, email, barangay);
        await SetPasswordForUser(userManager, user, password);
        await AddRolesToUser(
            userManager,
            roles,
            user);
    }

    private static async Task CreateBlotters(ApplicationDbContext dbContext, Barangay barangay)
    {
        if (!await dbContext.Blotters.AnyAsync())
        {
            var random = new Random();

            var blotters = Enumerable.Range(1, 10).Select(x => new Blotter
            {
                BarangayId = barangay.Id,
                Complainant = DataGenerator.GenerateString(),
                DateTime = DataGenerator.GenerateDateTime(),
                Location = DataGenerator.GenerateString(),
                Details = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.\r\n\r\nThe standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                BlotterType = "Physical Assault",
                Respondent = DataGenerator.GenerateString(),
                Status = Enum.GetValues<BlotterStatus>()[random.Next(0, 1)]
            });

            await dbContext.AddRangeAsync(blotters);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task CreateResidents(ApplicationDbContext dbContext, Barangay barangay)
    {
        var addresses = new List<Address>();

        if (!await dbContext.Addresses.AnyAsync())
        {
            addresses = Enumerable.Range(1, 7).Select(x => new Address
            {
                StreetAddress = DataGenerator.GenerateString(),
                BarangayId = barangay.Id,
                Purok = $"Purok {x}",
            }).ToList();

            await dbContext.AddRangeAsync(addresses);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            addresses = await dbContext.Addresses.ToListAsync();
        }

        if (!await dbContext.Residents.AnyAsync())
        {
            var random = new Random();

            var residents = Enumerable.Range(1, 15).Select(x => new Resident
            {
                IsPwd = x % 2 == 0,
                Disability = x % 2 == 0 ? DataGenerator.GenerateString() : null,
                Address = addresses[random.Next(0, 6)],
                Gender = Enum.GetValues<Gender>()[random.Next(0, 2)],
                Birthdate = DataGenerator.GenerateDateTime(),
                FirstName = DataGenerator.GenerateString(),
                CivilStatus = Enum.GetValues<CivilStatus>()[random.Next(0, 3)],
                ContactNo = DataGenerator.GenerateString(),
                Email = DataGenerator.GenerateString(),
                LastName = DataGenerator.GenerateString(),
                MiddleName = DataGenerator.GenerateString(),
                VoterStatus = Enum.GetValues<VoterStatus>()[random.Next(0, 1)],
                ImageUrl = "1617847311268.jpg"
            });

            await dbContext.AddRangeAsync(residents);
            await dbContext.SaveChangesAsync();
        }

    }

    private static async Task CreateRoles(
        RoleManager<IdentityRole> roleManager,
        List<string> roles)
    {
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> userManager, string email, Barangay barangay)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user);

        await userManager.AddClaimAsync(user, new Claim(Claims.Barangay, barangay.Id.ToString()));

        return user;
    }

    private static async Task<Barangay> CreateBarangay(ApplicationDbContext dbContext)
    {
        var officials = new List<Official>
        {
            new()
            {
                Name = "Alexander",
                Position = "Barangay Captain",
                Image = "1617847311268.jpg",
            },
            new()
            {
                Name = "Alexander",
                Position = "Secretary",
                Image = "1617847311268.jpg"
            },
            new()
            {
                Name = "Alexander",
                Position = "Treasurer",
                Image = "1617847311268.jpg"
            },
        };

        officials.AddRange(Enumerable.Range(1,7).Select(x => new Official
        {
            Name = "Alexander",
            Position = $"Kagawad #{x}",
            Image = "1617847311268.jpg"
        }));

        var barangay = new Barangay
        {
            Name = "Barangay Centro",
            Municipality = "Camasi",
            Province = "Cagayan Valley",
            ContactNo = "091235456",
            Officials = JsonSerializer.Serialize(officials),
            Logo = "logo.png"
        };

        dbContext.Add(barangay);
        await dbContext.SaveChangesAsync();

        return barangay;
    }

    private static async Task SetPasswordForUser(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password)
    {
        await userManager.AddPasswordAsync(user, password);
    }

    private static async Task AddRolesToUser(
        UserManager<ApplicationUser> userManager,
        string[] roles,
        ApplicationUser user)
    {
        foreach (var role in roles)
        {
            var ir = await userManager.AddToRoleAsync(user, role);
        }
    }
}
