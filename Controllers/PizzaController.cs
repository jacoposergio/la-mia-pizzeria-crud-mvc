using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

            List<Pizza> listaPizza = db.Pizze.Include(pizza => pizza.Category).ToList();

            return View(listaPizza);
        }

        public IActionResult Detail(int id)
        {

            Pizza pizza = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
            return View(pizza);
        }

        public IActionResult Create()
        {
            PizzaForm formData = new PizzaForm();
            formData.Pizza = new Pizza();
            formData.Categories = db.Categories.ToList();

            //formData a questo punto diventa il nuovo model
            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaForm formData)
        {
            
            if (!ModelState.IsValid)
            {
                //PizzaForm formData = new PostForm();
                //formData.Pizza = pizza;
                formData.Categories = db.Categories.ToList();
                return View(formData);  //devo riadattare il model per passarlo
            }

            db.Pizze.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int Id)
        {
            formData.Pizza.Id = Id;
            //PizzaForm formData = new PizzaForm();
            //formData.Pizza = new Pizza();
            Pizza pizza = db.Pizze.Where(pizza => pizza.Id == Id).FirstOrDefault();

            if (pizza == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();

           

            return View(formData);
        }

        [HttpPost]
        public IActionResult Update(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Pizze.Update(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id)
        {
            Pizza pizza = db.Pizze.Where(pizza => pizza.Id == Id).FirstOrDefault();

            if (pizza == null)
            {
                return NotFound();
            }
            db.Pizze.Remove(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
