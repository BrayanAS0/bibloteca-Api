using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.DTOS;

public class ComentarioDto{
    public Guid Id {get;set;}
    [Required]
    public required string Cuerpo {set;get;}
    public DateTime FechaDePublicacion {get;set;}
    public required string UsuarioId {get;set;}
    public required string UsuarioEmail { get; set; }

}