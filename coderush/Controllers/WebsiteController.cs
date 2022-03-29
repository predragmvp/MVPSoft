using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    public class WebsiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
