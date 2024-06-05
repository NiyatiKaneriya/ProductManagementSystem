using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult ErrorIndex()
        {
            return View();
        }

        [Route("/Error/404")]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}
