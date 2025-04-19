using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.DTOS;

public class ComentarioCreacionDto{
    

    [Required]
    public required string Cuerpo {set;get;}
public Guid Id = new Guid();

}