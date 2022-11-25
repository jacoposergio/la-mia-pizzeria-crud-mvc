using la_mia_pizzeria_static.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
