
using System.ComponentModel.DataAnnotations;
using biblotecaApi.Validaciones;

namespace biblotecaApi.Entidades;
public class Autor
{
    public int id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        //[PrimeraLetraMayuscula]
        public required string Nombres { get; set; }

        [Required]
        [StringLength(150)]
        public required string Apellidos {get;set;}
        public string? Identificacion {get;set;}
        public List<AutorLibro> Autores {get;set;}=[];

        public List<Libro> Libros { get; set; } = new List<Libro>();

}

