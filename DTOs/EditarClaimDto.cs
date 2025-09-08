using System.ComponentModel.DataAnnotations;

namespace bibloteca_api.DTOs
{
    public class EditarClaimDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
