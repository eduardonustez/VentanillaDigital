using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using System;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas
{
    public interface IUsuarioAdministracionRepositorio : IRepositorioBase<UsuarioAdministracion>, IDisposable
    {
        Task<UsuarioAdministracion> ObtenerUsuarioAdministracion(string login);
    }
}
