using System.Security.Claims;
using System.Text.Json;
using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Setup;

[AllowAnonymous]
[Route("[controller]")]
public class SetupController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SetupController(
        IWebHostEnvironment webHostEnvironment,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
    {
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Setup()
    {
        return View(new SetupViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> Setup(SetupViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        var officials = model.Officials
            .Select(x => new Official
            {
                Name = x.Name,
                Position = x.Position,
                Image = x.Image?.Length > 0 ? $"{Guid.NewGuid()}{Path.GetExtension(x.Image.FileName)}" : string.Empty
            })
            .ToList();

        var barangay = new Barangay
        {
            Name = model.Name,
            Logo = $"{Guid.NewGuid()}{Path.GetExtension(model.Logo.FileName)}",
            Officials = JsonSerializer.Serialize(officials)
        };

        _context.Add(barangay);
        await _context.SaveChangesAsync();

        var user = new ApplicationUser
        {
            UserName = model.AdminUsername,
            BarangayId = barangay.Id
        };

        var userResult = await _userManager.CreateAsync(user, model.AdminPassword);
        
        if (!userResult.Succeeded)
        {
            foreach (var error in userResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _userManager.AddClaimAsync(user, new Claim(Claims.Barangay, barangay.Id.ToString()));

        var roleExists = await _roleManager.RoleExistsAsync(Roles.Admin);

        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        await _userManager.AddToRoleAsync(user, Roles.Admin);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        foreach (var official in from official in model.Officials let imagePath = officials[model.Officials.IndexOf(official)].Image where !string.IsNullOrEmpty(imagePath) select official)
        {
            await using var officialStream = new FileStream(Path.Combine(path, officials[model.Officials.IndexOf(official)].Image), FileMode.Create);
            await official.Image.CopyToAsync(officialStream);
        }

        var logoPath = Path.Combine(path, $"{Guid.NewGuid()}{Path.GetExtension(model.Logo.FileName)}");

        await using var stream = new FileStream(logoPath, FileMode.Create);
        await model.Logo.CopyToAsync(stream);

        await _signInManager.SignInAsync(user, true);

        return RedirectToAction("Dashboard", "Dashboard");
    }
}
