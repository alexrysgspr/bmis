using System.Security.Claims;
using System.Text.Json;
using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.Web.Models;
using Bmis.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Setup;

[AllowAnonymous]
[Route("[controller]")]
public class SetupController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IFileServices _fileServices;

    public SetupController(
        
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context,
        IFileServices fileServices)
    {
        _fileServices = fileServices;
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
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

        await _userManager.AddToRoleAsync(user, Roles.Admin);

        foreach (var official in from official in model.Officials let imagePath = officials[model.Officials.IndexOf(official)].Image where !string.IsNullOrEmpty(imagePath) select official)
        {
            await _fileServices.Add(official.Image.OpenReadStream(),
                officials[model.Officials.IndexOf(official)].Image);
        }

        await _fileServices.Add(model.Logo.OpenReadStream(),
            barangay.Logo);

        await _signInManager.SignInAsync(user, true);

        return RedirectToAction("Dashboard", "Dashboard");
    }
}
