using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace biblotecaApi.Validaciones;

public class PrimeraDosLetrasMayusculas:ValidationAttribute{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
if(value is null || value.ToString().IsNullOrEmpty())
return  ValidationResult.Success;

if(value.ToString()!.Length <=1)
return new ValidationResult("Debe contener 2 o mas caracteres");
string word = value.ToString()!.Substring(0,2); 

string wordUpperCase= value.ToString()!.Substring(0,2).ToUpper();
if(word==wordUpperCase)
return ValidationResult.Success;
else
return new ValidationResult(nameof(value) +" debe tener las 2 priemras letras mayusculas");






    }


}