using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    public class WebsiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Development()
        {
            return View();
        }
        public IActionResult QA()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
