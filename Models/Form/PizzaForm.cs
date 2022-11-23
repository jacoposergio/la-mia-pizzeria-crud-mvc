namespace la_mia_pizzeria_static.Models.Form
{
    public class PizzaForm
    {
        //Model del db e delle views: per la creazione di create,read, update
        public Pizza Pizza { get; set; }

        //questa classe è a contorno, ci serve per fare cose nella views
        //è opzionale perchè in fase di validazione mi devo occupare solo del db
        public List<Category>? Categories { get; set; }
    }
}
