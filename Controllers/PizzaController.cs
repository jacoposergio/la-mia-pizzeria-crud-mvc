using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {

        PizzeriaDbContext db;
        public PizzaController() : base()
        {
            db = new PizzeriaDbContext();
        }

        public IActionResult Index()
        {

            List<Pizza> listaPizza = db.Pizze.ToList();

            return View(listaPizza);
        }

        public IActionResult Detail(int id)
        {

            Pizza pizza = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
            return View(pizza);
        }

        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Pizze.Add(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
