using System.Net;
using Bmis.EntityFramework.DesignTime;
using Bmis.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Residents
{
    [Route("[controller]")]
    public class ResidentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        [TempData]
        public string StatusMessage { get; set; }

        public ResidentsController(
            ApplicationDbContext context,
            IFileServices fileServices)
        {
            _fileServices = fileServices;
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
        public async Task<ActionResult> Add(ResidentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors =  ModelState.ToDictionary(kvp => kvp.Key,
                        kvp => kvp.Value.Errors
                            .Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());

                return Json(errors.ToDictionary(x => x.Key, x => x.Value.First()));
            }

            var fileName = string.Empty;

            if (model.Image != null)
            {
                fileName = $"{Guid.NewGuid()}.jpg";
                await _fileServices.Add(model.Image.OpenReadStream(), fileName);
            }

            var resident = model.ToResident(fileName);

            _context.Add(resident);
            await _context.SaveChangesAsync();

            if (model.Image != null)
            {
                await _fileServices.Add(model.Image.OpenReadStream(), $"{Guid.NewGuid()}.jpg");
            }

            StatusMessage = $"'{model.GetFullName()}' added successfully.";

            return Created(nameof(Resident), new { id = resident.Id });
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _context.Residents.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            StatusMessage = $"'{entity.FirstName}' deleted successfully.";

            return RedirectToAction(nameof(Residents));
        }
    }
}
