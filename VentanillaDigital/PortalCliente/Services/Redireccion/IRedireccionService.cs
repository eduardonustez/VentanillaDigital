using PortalCliente.Data.Account;
using System.Threading.Tasks;

namespace PortalCliente.Services.RedireccionLogin
{
    public interface IRedireccionService
    {
        /// <summary>
        /// Obtiene la ruta al que se redirige al usuario luego de iniciar sesi�n.
        /// Verifica si ya tiene asignadas firma y pin de acuerdo al rol.
        /// </summary>
        /// <param name="usuarioAutenticado">Informaci�n del usuario</param>
        /// <returns>Ruta a navegar seg�n rol</returns>
        public Task IrAPaginaInicial();
    }
}