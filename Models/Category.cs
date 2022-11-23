namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //relazione 1 a * con Pizza
        public List<Pizza> Pizze { get; set; }
    }
}
