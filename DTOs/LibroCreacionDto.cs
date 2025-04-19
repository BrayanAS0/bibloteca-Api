using System.ComponentModel.DataAnnotations;
using biblotecaApi.Entidades;

namespace biblotecaApi.DTOS;
public class LibroCreacionDto{
    [Required]
        public required string Titulo {get;set;}
       public List<int> AutoresIds {get;set;}=[];
}