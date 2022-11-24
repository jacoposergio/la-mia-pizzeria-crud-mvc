using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

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

            Pizza pizza = db.Pizze.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();

            if(pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }

        public IActionResult Create()
        {
            PizzaForm formData = new PizzaForm();

            formData.Pizza = new Pizza();
            formData.Categories = db.Categories.ToList();
            formData.Ingredients = new List<SelectListItem>();

            List<Ingredient> ingredientList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientList)
            {
                formData.Ingredients.Add(new SelectListItem(ingredient.Title, ingredient.Id.ToString()));
            }

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
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> tagList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in tagList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Title, ingredient.Id.ToString()));
                }

                return View(formData);  //devo riadattare il model per passarlo
            }

            //associazione degli ingredienti scelti nella create al modello
            formData.Pizza.Ingredients = new List<Ingredient>(); //è opzionale e null, lo inizializzo fuori al foreach

            if (formData.SelectedIngredients != null)
            {

                foreach (int ingID in formData.SelectedIngredients)
                {
                    Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingID).FirstOrDefault();
                    formData.Pizza.Ingredients.Add(ingredient);
                }
            }


            db.Pizze.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {

            Pizza pizza = db.Pizze.Where(pizza => pizza.Id == id).Include("Ingredients").FirstOrDefault();

            if (pizza == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();
            formData.Ingredients = new List<SelectListItem>();

            List<Ingredient> ingredientsList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientsList)
            {
                formData.Ingredients.Add(new SelectListItem(
                    ingredient.Title,
                    ingredient.Id.ToString(),
                    pizza.Ingredients.Any(i => i.Id == ingredient.Id)
                   ));
            }


            return View(formData);
        }

        //altro modo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaForm formData)
        {          

            if (!ModelState.IsValid)
            {
                formData.Pizza.Id = id;
                //return View(pizzaItem);
                formData.Categories = db.Categories.ToList();
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> ingredientsList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientsList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Title, ingredient.Id.ToString()));
                }


                return View(formData);
            }

            //update esplicito con nuovo oggetto
            Pizza pizzaItem = db.Pizze.Where(pizza => pizza.Id == id).Include("Ingredients").FirstOrDefault();

            if (pizzaItem == null)
            {
                return NotFound();
            }


            pizzaItem.Name = formData.Pizza.Name;
            pizzaItem.Description = formData.Pizza.Description;
            pizzaItem.Image = formData.Pizza.Image;
            pizzaItem.Price = formData.Pizza.Price;
            pizzaItem.CategoryId = formData.Pizza.CategoryId;

            pizzaItem.Ingredients.Clear();

            if (formData.SelectedIngredients == null)
            {
                formData.SelectedIngredients = new List<int>();
            }

            foreach (int ingredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizzaItem.Ingredients.Add(ingredient);
            }

            //db.Posts.Update(formData.Post);
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



//1 
//    2
//    2
//    3
//    4