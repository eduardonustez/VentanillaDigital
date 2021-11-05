using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface INotariaRepositorio : IRepositorioBase<Notaria>, IDisposable
    {
        Task<Guid> CrearPersonaYNotariaUsuario(Persona persona, NotariaUsuarios notariaUsuarios, List<PersonaDatos> personaDatos);
        Task<ConvenioRNEC> ObtenerConvenioRNEC(long notariaID);
        Task<IEnumerable<NotariaUsuarios>> ObtenerListadoUsuariosNotaria(long NotariaId);
        Persona obtenerPersonaId(long personaId);
        Task<bool> ActualizarSincronizacionRNEC(long notariaUsuarioId);
        NotariaUsuarios obtenerNotariaUsuarioxEmail(string Email);
        NotariaUsuarios ObtenerUsuarioNotariaPorId(Guid UsuarioId);
        Task<List<NotariaCliente>> Obtener();
    }
}
