using System.Text.Json;
using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers;


[Route("[controller]")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly ApplicationDbContext _context;

    public DashboardController(
        ILogger<DashboardController> logger,
        ApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }


    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var barangayId = User.GetBarangayId();

        var barangay = await _context.Barangays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == barangayId);

        if (barangay == default)
        {
            return NotFound();
        }

        var model = new DashboardViewModel
        {
            BarangayName = barangay.Name,
            Officials = JsonSerializer.Deserialize<List<Official>>(barangay.Officials)
        };

        model.TotalResidents = await _context
            .Addresses
            .Include(x => x.Residents)
            .AsNoTracking()
            .SumAsync(x => x.Residents.Count);

        model.TotalActiveVoters = await _context
            .Residents
            .AsNoTracking()
            .CountAsync(x => x.VoterStatus == VoterStatus.Active);

        model.TotalPwd = await _context
            .Residents
            .AsNoTracking()
            .CountAsync(x => x.IsPwd);

        var purokPopulation = await _context
            .Addresses
            .Include(x => x.Residents)
            .AsNoTracking()
            .Select(x => new
            {
                x.Purok,
                Count = x.Residents.Count()
            })
            .ToListAsync();

        model.PurokPopulations = purokPopulation
            .GroupBy(x => x.Purok)
            .Select(x => new PurokPopulation
            {
                Purok = x.Key,
                Total = x.Sum(y => y.Count)
            })
            .ToList();

        model.PopulationClassifications = await _context
            .Residents
            .AsNoTracking()
            .GroupBy(x => x.Gender)
            .Select(y => new PopulationClassification
            {
                Total = y.Count(),
                Key = y.Key.ToString()
            })
            .ToListAsync();

        model.PopulationClassifications.Add(new PopulationClassification
        {
            Key = "PWD",
            Total = model.TotalPwd
        });

        return View(model);
    }

}
