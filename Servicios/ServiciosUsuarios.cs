using Microsoft.AspNetCore.Identity;

namespace bibloteca_api.Servicios
{
    public class ServiciosUsuarios : IServiciosUsuarios
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ServiciosUsuarios(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<IdentityUser?> ObetenerUsuario()
        {
            var emailClaim = _contextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            if (emailClaim == null) return null;
            var email = emailClaim.Value;

            return await _userManager.FindByEmailAsync(email);

        }

    }
    }
