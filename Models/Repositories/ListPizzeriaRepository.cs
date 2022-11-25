using la_mia_pizzeria_static.Models.Form;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models.Repositories
{
    public class ListPizzeriaRepository : IPizzeriaRepository
    {
        //se non abbiamo più il server lavoreremo su questa lista di post
        public static List<Pizza> Pizze = new List<Pizza>();

        public ListPizzeriaRepository()
        {
           //ogni nuova istanza cancella la lista, quindi nn possiamo farlo
            //Pizze = new List<Pizza>();
        }
        public List<Pizza> All()
        {
            return ListPizzeriaRepository.Pizze;
        }

        public void Create(Pizza pizza, List<int> SelectedIngredients)
        {
            pizza.Id = Pizze.Count;  //simula il pk
            pizza.Category = new Category() { Id = 1, Title = "Fake Category" };
            //non potendo avere il db e nn potendo avere l'ingredients list
            pizza.Ingredients = new List<Ingredient> (); 
           foreach(int IngredientId in SelectedIngredients)
            {
                pizza.Ingredients.Add(new Ingredient() { Id = IngredientId , Title = "Fake ingredient" + IngredientId }); 
            }
           Pizze.Add(pizza);
        }

        public Pizza GetById(int id)
        {
            Pizza pizza = Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();
            pizza.Category = new Category() { Id = 1, Title = "Fake Category" };
            return pizza;
        }

        public void Update(Pizza pizza, Pizza formData, List<int>? SelectedIngredients)
        {
            pizza = formData;
            pizza.Category = new Category() { Id = 1, Title = "Fake Category" };
            pizza.Ingredients = new List<Ingredient>();
            //simulazione da implementare con listtagrepository
            foreach(int IngredientId in SelectedIngredients)
            {
                pizza.Ingredients.Add(new Ingredient() { Id = IngredientId, Title = "Fake ingredient" + IngredientId });
            }
        }

        public void Delete(Pizza pizza)
        {
            Pizze.Remove(pizza);
        }
    }
}
