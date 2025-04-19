using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.Validaciones;

public class PrimeraLetraAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrEmpty(value.ToString()))
            return ValidationResult.Success;

            var primeraLetra =value.ToString()![0].ToString();
            if(primeraLetra!= primeraLetra.ToUpper())
            return new ValidationResult("La primera letra debe ser mayuscula");
            else
            return ValidationResult.Success;
    }

}