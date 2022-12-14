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
                    Address = x.StreetAddress,
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
                Address = household.StreetAddress,
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
                .Where(x => x.StreetAddress.StartsWith(query, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new { x.Id, x.StreetAddress, x.Purok })
                .ToListAsync();

            return Json(household);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{entity.StreetAddress}' deleted successfully.";

            return RedirectToAction(nameof(Households));
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult> Update(int id, HouseHoldViewModel model)
        {
            var entity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Purok = model.Purok;
            entity.StreetAddress = model.Address;
            _context.Update(entity);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{entity.StreetAddress}' updated successfully.";

            return RedirectToAction(nameof(Households));
        }
    }
}
