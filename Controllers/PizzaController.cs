using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
            formData.Ingredients = db.Ingredients.ToList();

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

            //associazione degli ingredienti scelti nella create al modello
            formData.Pizza.Ingredients = new List<Ingredient>(); //è opzionale e null, lo inizializzo fuori al foreach
            foreach(int IngredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == IngredientId).FirstOrDefault();
                return View(formData);
            }

            db.Pizze.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {

            Pizza post = db.Pizze.Where(post => post.Id == id).FirstOrDefault();

            if (post == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = post;
            formData.Categories = db.Categories.ToList();

            return View(formData);
        }

        //altro modo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaForm formData)
        {

            formData.Pizza.Id = id;

            if (!ModelState.IsValid)
            {
                //return View(postItem);
                formData.Categories = db.Categories.ToList();
                return View(formData);
            }

            db.Pizze.Update(formData.Pizza);
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
