using Bmis.EntityFramework.DesignTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Blotters
{
    [Route("[controller]")]
    public class BlottersController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public BlottersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Blotters()
        {
            var blotters = await _context
                .Blotters
                .AsNoTracking()
                .ToListAsync();

            return View(blotters.Select(x => x.ToViewModel()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Blotter(int id)
        {
            var blotter = await _context.Blotters.FirstOrDefaultAsync(x => x.Id == id);

            if (blotter == null)
            {
                return NotFound();
            }

            return View(blotter.ToViewModel());
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Blotter(BlotterViewModel model, int id)
        {
            var blotter = await _context.Blotters.FirstOrDefaultAsync(x => x.Id == id);

            if (blotter == null)
            {
                return NotFound();
            }

            blotter.ToUpdatedBlotter(model);

            _context.Update(blotter);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{model.Complainant}' blotter updated successfully.";

            return RedirectToAction(nameof(Blotters));
        }

        [HttpGet("[action]")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(BlotterViewModel model)
        {
            var blotter = model.ToBlotter();

            _context.Add(blotter);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{model.Complainant}' blotter added successfully.";

            return RedirectToAction(nameof(Blotters));
        }
    }
}
