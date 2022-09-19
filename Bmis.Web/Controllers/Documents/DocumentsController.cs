using Bmis.EntityFramework.DesignTime;
using Bmis.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Bmis.Web.Controllers.Documents
{
    [Route("[controller]")]
    public class DocumentsController : Controller
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public DocumentsController(
            IRazorViewEngine razorViewEngine,
            ApplicationDbContext dbContext,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
            _dbContext = dbContext;
            _razorViewEngine = razorViewEngine;
        }

        [HttpGet("certificate-of-indigency")]
        public IActionResult CertificateOfIndigency()
        {
            return View();
        }

        [HttpGet("barangay-clearance")]
        public IActionResult BarangayClearance()
        {
            return View();
        }

        [HttpGet("document-header")]
        public async Task<string> DocumentHeader()
        {
            var model = await _dbContext.Barangays.FirstAsync(x => x.Id == User.GetBarangayId());

            await using var sw = new StringWriter();
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var viewName = "_DocumentHeader";
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewName} does not match any available view");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();

            //return View("_DocumentHeader");
        }
    }
}
