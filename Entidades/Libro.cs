using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.Entidades;
public class Libro{

    public int id {get;set;}
    [Required]
        [StringLength(250,ErrorMessage ="no debe ser mas de {1} caracteres el campo {0}")]
    public required string Titulo {get;set;}

    public List<AutorLibro> Autores {get;set;}=[];
    public List<Comentario> Comentarios {get;set;}=[];
}