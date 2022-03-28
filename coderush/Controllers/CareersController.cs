using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Controllers
{
    public class CareersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
