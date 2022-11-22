using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class RazorController : Controller
    {
        public IActionResult Snacks()
        {
            return View();
        }
    }
}
