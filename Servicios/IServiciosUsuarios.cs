using bibloteca_api.Entidades;
using Microsoft.AspNetCore.Identity;

namespace bibloteca_api.Servicios
{
    public interface IServiciosUsuarios
    {
        Task<Usuario?> ObetenerUsuario();
        Task<List<Usuario>> ObtenerUsuarios();
    }
}