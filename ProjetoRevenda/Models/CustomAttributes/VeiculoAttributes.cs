using System.ComponentModel.DataAnnotations;

namespace ProjetoRevenda.Models
{
    public class VeiculoAnoValidoAttribute : ValidationAttribute
    {
        int intAnoMin;
        int intAnoMax;
        public VeiculoAnoValidoAttribute(int pAnoMin)
        {
            intAnoMin = pAnoMin;
            intAnoMax = DateTime.Now.Year + 1;
        }

        public string GetErrorMessage() => $"O valor deve estar entre {intAnoMin} e {intAnoMax}.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if((int)value > intAnoMax || (int)value < intAnoMin)
            {
                return new ValidationResult(GetErrorMessage());
            }
                        
            return ValidationResult.Success;
        }

    }
}
