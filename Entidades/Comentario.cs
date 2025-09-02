using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.Entidades;
public class Comentario {
    public Guid Id {get;set;}
    [Required]
    public required string Cuerpo {set;get;}
    public DateTime FechaDePublicacion {get;set;}=DateTime.Now;
    public int LibroId {get;set;}
    public Libro? Libro {get;set;}
    public required string UsuarioId {get;set;}
    public IdentityUser? Usuario {get;set;}
}