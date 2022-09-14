using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.EntityFramework.Extensions;
using Bmis.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
                action => action.MigrationsAssembly("Bmis.EntityFramework"));
        })
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add services to the container.
builder
    .Services
    .AddAppPersistence(builder.Configuration)
    .AddRouting(options => options.LowercaseUrls = true)
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Account/Manage");
    options.Conventions.AuthorizePage("/Account/Logout");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
    });


await app.MigrateDatabaseAsync();

app.Run();
