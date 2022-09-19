using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.EntityFramework.Extensions;
using Bmis.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseInMemoryDatabase("bmis");

            //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
            //    action => action.MigrationsAssembly("Bmis.EntityFramework"));
        })
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
    {
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.Cookie.Name = "auth.bmis";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });

builder
    .Services
    .AddAppPersistence(builder.Configuration)
    .AddRouting(options => options.LowercaseUrls = true)
    .AddControllersWithViews(x => x.Filters.Add(new AuthorizeFilter()))
    .AddRazorRuntimeCompilation();


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
app.UseAuthentication(); ;
app.UseAuthorization();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });


await app.MigrateDatabaseAsync();

app.Run();
