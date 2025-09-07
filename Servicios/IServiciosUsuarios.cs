using Microsoft.AspNetCore.Identity;

namespace bibloteca_api.Servicios
{
    public interface IServiciosUsuarios
    {
        Task<IdentityUser?> ObetenerUsuario();
        Task<List<IdentityUser>> ObtenerUsuarios();
    }
}