using System.ComponentModel.DataAnnotations;
using biblotecaApi.Entidades;

namespace biblotecaApi.DTOS;

public class LibroDto{
    public int id {get;set;}
        [Required]
        [StringLength(250,ErrorMessage ="no debe ser mas de {1} caracteres el campo {0}")]
    public required string Titulo {get;set;}
}