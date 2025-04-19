using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.DTOS;
public class AutorCreactionDto{
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Nombres { get; set; }
        [Required]
        [StringLength(150)]
        public required string Apellidos {get;set;}
        public string? Identificacion {get;set;}
}