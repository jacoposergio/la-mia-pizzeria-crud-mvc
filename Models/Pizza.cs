using la_mia_pizzeria_static.Validator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Devi inserire un nome")]
        [StringLength(15, ErrorMessage = " Il nome deve essere inferiore a 15 caratteri")]
        
        public string Name { get; set; }
        [Column(TypeName = "Text")]
        [AlmenoCinqueParole]
        [Required(ErrorMessage = "Devi inserire una descrizione")]
        [StringLength(70, ErrorMessage = "La descrizione deve essere inferiore a 50 caratteri")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Devi inserire un prezzo")]
   

        [Range(1, 99.99, ErrorMessage = "Il Prezzo deve essere compreso tra 1 e 99 euro")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Devi inserire un Url di un'immagine")]
        public string Image { get; set; }

        //relazione 1 a * con Category
        public int CategoryId{ get; set; }
        public Category Category { get; set; }

        public Pizza()
        {

        }
        public Pizza(string name, string description, double price, string image)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
        }

    }



   
}
