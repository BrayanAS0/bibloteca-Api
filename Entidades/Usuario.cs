using Microsoft.AspNetCore.Identity;

namespace bibloteca_api.Entidades
{
    public class Usuario:IdentityUser
    {
        public DateTime FechaNacimiento { get; set; }
    }
}
