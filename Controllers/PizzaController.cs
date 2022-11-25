using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using la_mia_pizzeria_static.Models.Repositories;
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

        //Uso il repository al posto del db
        DbPizzeriaRepository pizzaRepository;
        public PizzaController() : base()
        {
            db = new PizzeriaDbContext();

            pizzaRepository = new DbPizzeriaRepository();
        }

        public IActionResult Index()
        {

            List<Pizza> listaPizza = pizzaRepository.All();

            return View(listaPizza);
        }

        public IActionResult Detail(int id)
        {

            Pizza pizza = pizzaRepository.GetById(id);

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
            formData.Ingredients = new List<SelectListItem>();  //ora gli ingredient sono una lista di SelectListItem

            //per popolarlo prendiamo una lista di ingredient e col forech la convertiamo
            List<Ingredient> ingredientList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientList)
            {
                formData.Ingredients.Add(new SelectListItem(ingredient.Title, ingredient.Id.ToString()));  //passiamo alla SelectListItemi i dati (le chiavi valore delle option)
                //Dato che convertiamo l'int in string avremo problemi sull altro oggetto
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
                
                formData.Categories = db.Categories.ToList();
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> IngredientList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in IngredientList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Title, ingredient.Id.ToString()));
                }

                return View(formData);  //devo riadattare il model per passarlo
            }

            //associazione degli ingredienti scelti nella create al modello
            formData.Pizza.Ingredients = new List<Ingredient>(); //è opzionale e null, lo inizializzo fuori al foreach

            if (formData.SelectedIngredients != null)
            {

                foreach (int ingredientId in formData.SelectedIngredients)
                {
                    Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                    formData.Pizza.Ingredients.Add(ingredient);
                }
            }


            db.Pizze.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            //Non avendo la ingredientId(relazione * a *), dobbiamo recuperare selectedTags e Model.Tag che passiamo alla view
            Pizza pizza = pizzaRepository.GetById(id);

            if (pizza == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();
            formData.Ingredients = new List<SelectListItem>();

            List<Ingredient> ingredientsList = db.Ingredients.ToList();  //creiamo una lista che contiene tutti i tag
            //nel foreach creiamo l'oggetto che contiene i nostri dati e  tutti gli ingredienti già selezionati  
            //(non abbiamo l'hold, quindi questo è un escamotage per recuperare gli ingredienti  già salvati

            foreach (Ingredient ingredient in ingredientsList)
            {
                formData.Ingredients.Add(new SelectListItem(
                    ingredient.Title,
                    ingredient.Id.ToString(),
                    pizza.Ingredients.Any(i => i.Id == ingredient.Id)  //se l'id è true significa che esiste nella pivot e quindi passa all'asp item che lo stampa
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

            //update esplicito con nuovo oggetto: recuperiamo la pizza dal db
            Pizza pizzaItem = pizzaRepository.GetById(id);
            //ora tutti gli elementi sono tracked, cioè stiamo lavorando sul db

            if (pizzaItem == null)
            {
                return NotFound();
            }

            //aggiorniamo tutti i dati
            pizzaItem.Name = formData.Pizza.Name;
            pizzaItem.Description = formData.Pizza.Description;
            pizzaItem.Image = formData.Pizza.Image;
            pizzaItem.Price = formData.Pizza.Price;
            pizzaItem.CategoryId = formData.Pizza.CategoryId;

            pizzaItem.Ingredients.Clear(); //cancelliamo le relazioni che già esistevano

            //Update implicito

            if (formData.SelectedIngredients == null)
            {
                formData.SelectedIngredients = new List<int>();
            }

            foreach (int ingredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizzaItem.Ingredients.Add(ingredient);   //adesso possiamo riassegnarli facendo una query, quando fa l'add sa che  è un update e non new tag
                // non viene creato nuovo, ma assegnato alla pivot
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
