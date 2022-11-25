using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace la_mia_pizzeria_static.Models.Repositories
{
    public class DbPizzeriaRepository
    {
        private PizzeriaDbContext db;

        public DbPizzeriaRepository()
        {
            db = new PizzeriaDbContext();
        }

        // mi interessa che questa funzione restituisca una lista di post,
        //quindi il controller è più sicuro perchè deve richiamare solo la funzione
        public List<Pizza> All()
        {
            return db.Pizze.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();
        }

        public Pizza GetById(int id)
        {
           return db.Pizze.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();
        }

        public PizzaForm CreatePizzaForm()
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
            return formData;
        }

        public void Create(Pizza pizza, List<int> SelectedIngredients)
        {
            //associazione degli ingredienti scelti nella create al modello
            pizza.Ingredients = new List<Ingredient>(); //è opzionale e null, lo inizializzo fuori al foreach


            foreach (int ingredientId in SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);
            }

            db.Pizze.Add(pizza);
            db.SaveChanges();
        }

        public void Update(Pizza pizza, Pizza formData, List<int>? SelectedIngredients)
        {
          
            //Update implicito

            if (SelectedIngredients == null)
            {
                SelectedIngredients = new List<int>();
            }

            //aggiorniamo tutti i dati
            pizza.Name = formData.Name;
            pizza.Description = formData.Description;
            pizza.Image = formData.Image;
            pizza.Price = formData.Price;
            pizza.CategoryId = formData.CategoryId;

            pizza.Ingredients.Clear(); //cancelliamo le relazioni che già esistevano

     

            foreach (int ingredientId in SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);   //adesso possiamo riassegnarli facendo una query, quando fa l'add sa che  è un update e non new tag
                // non viene creato nuovo, ma assegnato alla pivot
            }

            //db.Posts.Update(formData.Post);
            db.SaveChanges();
        }

        internal void Delete(Pizza pizza)
        {
            //puo essere che debbano esserci dei controlli,
            //che ho lasciato nel delete del controller
            //ma comunque remove genera delle eccezioni
            db.Pizze.Remove(pizza);
            db.SaveChanges();
        }
    }
}
