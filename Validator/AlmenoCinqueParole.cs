using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Validator
{
    public class AlmenoCinqueParole : ValidationAttribute

    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if (fieldValue == null)
            {
                return new ValidationResult("Il campo descrizione è vuoto");
            }


            if (fieldValue.Trim().Count(c => c == ' ') < 4)
            {
                return new ValidationResult("Il campo descrizione deve avere almeno cinque parole");
            }

            return ValidationResult.Success;
        }
    }
}
