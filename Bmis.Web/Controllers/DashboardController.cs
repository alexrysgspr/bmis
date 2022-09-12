using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers;

[Route("[controller]")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly BmisDbContext _context;

    public DashboardController(
        ILogger<DashboardController> logger,
        BmisDbContext context)
    {
        _context = context;
        _logger = logger;
    }


    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {

        var model = new DashboardViewModel();

        model.TotalResidents = await _context
            .Residents
            .AsNoTracking()
            .CountAsync();

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
