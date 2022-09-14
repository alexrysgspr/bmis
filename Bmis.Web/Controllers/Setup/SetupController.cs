using Microsoft.AspNetCore.Mvc;

namespace Bmis.Web.Controllers.Setup;

[Route("[controller]")]
public class SetupController : Controller
{
    [HttpGet]
    public IActionResult Setup()
    {
        return View();
    }
}
