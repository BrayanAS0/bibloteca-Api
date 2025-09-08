using bibloteca_api.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bibloteca_api.Servicios
{
    public class ServiciosUsuarios : IServiciosUsuarios
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ServiciosUsuarios(UserManager<Usuario> userManager, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<Usuario?> ObetenerUsuario()
        {
            var emailClaim = _contextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            if (emailClaim == null) return null;
            var email = emailClaim.Value;

            return await _userManager.FindByEmailAsync(email);

        }
        public async Task<List<Usuario>> ObtenerUsuarios()
        {
            var usuarios =await  _userManager.Users.ToListAsync();
            return usuarios;
        }

    }
    }
