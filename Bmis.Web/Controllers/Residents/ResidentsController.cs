using Bmis.EntityFramework.DesignTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Residents
{
    [Route("[controller]")]
    public class ResidentsController : Controller
    {
        private readonly BmisDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public ResidentsController(BmisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Residents()
        {
            var residents = await _context
                .Residents
                .Include(x => x.Address)
                .AsNoTracking()
                .ToListAsync();

            return View(residents.Select(x => x.ToViewModel()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Resident(int id)
        {
            var resident = await _context
                .Residents
                .Include(x => x.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (resident == null)
            {
                return NotFound();
            }

            return View(resident.ToViewModel());
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Resident(ResidentViewModel model, int id)
        {
            var resident = await _context
                .Residents
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (resident == null)
            {
                return NotFound();
            }

            resident.ToUpdatedResident(model);

            _context.Update(resident);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{model.GetFullName()}' updated successfully.";

            return RedirectToAction(nameof(Residents));
        }

        [HttpGet("[action]")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(ResidentViewModel model)
        {
            var resident = model.ToResident();

            _context.Add(resident);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{model.GetFullName()}' added successfully.";

            return RedirectToAction(nameof(Residents));
        }
    }
}
