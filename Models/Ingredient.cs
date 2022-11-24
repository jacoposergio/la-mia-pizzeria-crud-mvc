namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Pizza>? Pizze { get; set; }  //relazioni molti a molti Ingrediente - Pizza
    }
}
