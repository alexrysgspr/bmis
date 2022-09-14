using Bmis.EntityFramework.DesignTime;
using Bmis.EntityFramework.Entities;
using Bmis.Web.Controllers.Residents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Households
{
    [Route("[controller]")]
    public class HouseholdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public HouseholdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Households()
        {
            var model = new HouseHoldViewModel();

            var households = await _context
                .Addresses
                .Include(x => x.Residents)
                .AsNoTracking()
                .Select(x => new HouseHoldViewModel
                {
                    AddressId = x.Id,
                    Address = x.AddressLine,
                    Purok = x.Purok,
                    TotalFemale = x.Residents.Count(y => y.Gender == Gender.Female),
                    TotalMale = x.Residents.Count(y => y.Gender == Gender.Male),
                    TotalPwd = x.Residents.Count(y => y.IsPwd),
                    TotalMembers = x.Residents.Count(),
                    TotalVoters = x.Residents.Count(x => x.IsPwd)
                })
                .ToListAsync();

            return View(households);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Household(int id)
        {
            var household = await _context
                .Addresses
                .Include(x => x.Residents)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (household == null)
            {
                return NotFound();
            }

            var model = new HouseHoldViewModel
            {
                AddressId = household.Id,
                Address = household.AddressLine,
                Purok = household.Purok,
                TotalFemale = household.Residents.Count(y => y.Gender == Gender.Female),
                TotalMale = household.Residents.Count(y => y.Gender == Gender.Male),
                TotalPwd = household.Residents.Count(y => y.IsPwd),
                TotalMembers = household.Residents.Count(),
                TotalVoters = household.Residents.Count(x => x.IsPwd),
                Residents = household.Residents.Select(x => x.ToViewModel()).ToList()
            };

            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string query)
        {
            var household = await _context
                .Addresses
                .AsNoTracking()
                .Where(x => x.AddressLine.StartsWith(query))
                .Select(x => new { x.Id, x.AddressLine, x.Purok })
                .ToListAsync();

            return Json(household);
        }
    }
}
