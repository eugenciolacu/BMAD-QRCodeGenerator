using Microsoft.AspNetCore.Mvc;

namespace QRCodeGeneratorApp.Controllers
{
    /// <summary>
    /// Handles requests for the Home/Index landing page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the application landing page.
        /// </summary>
        /// <returns>The Home/Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}

