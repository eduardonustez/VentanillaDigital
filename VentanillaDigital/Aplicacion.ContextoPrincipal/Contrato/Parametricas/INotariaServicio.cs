using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface INotariaServicio : IDisposable
    {
        Task<IEnumerable<NotariaForReturn>> Obtener();
        Task<NotariaForReturn> ObtenerNotariaPorUsuario(string userEmail);
        Task<ConfiguracionRNECDTO> ObtenerConfiguracionRNEC(long notariaID);
        Task<NotariaForReturn> ObtenerNotariaPorId(long NotariaId);
        Task<PersonasRequestDTO> CreacionPersonaYNotarioUsuario(PersonaCreateOrUpdateDTO persona, NotariaUsuarioCreateDTO notaria, List<PersonaDatosDTO> personaDatos,int RolId);
        Task<PersonasRequestDTO> ActualizacionPersonaYNotarioUsuario(PersonaCreateOrUpdateDTO persona, NotariaUsuarioCreateDTO notaria, List<PersonaDatosDTO> personaDatos, string userId, int RolId);
        Task<IEnumerable<NotariaUsuarios>> obtenerListaUsuarios(long NotariaId);
        Task<Persona> obtenerPersonaId(long personaId);
        Task<bool> ActualizarSincronizacionRNEC(long notariaUsuarioId);
        Task<NotariaUsuarios> obtenerNotariaUsuarioxEmail(string Email);
        Task<NotariaUsuarios> ObtenerUsuarioNotariaPorId(Guid UsuarioId);
        Task ActualizarSelloNotaria(SelloNotariaEditDto selloNotariaEditDto);
        void AgregarNotariaUsuario(NotariaUsuarios item);
        Task<bool> EliminarUsuario(UsuarioDeleteRequestDTO usuarioDeleteDto);
        Task<IEnumerable<NotariaClienteDTO>> ObtenerNotarias();
        Task<Guid> ObtenerConvenioRNEC(long notariaId);
        Task<IEnumerable<NotariaBasicDTO>> ObtenerTodasNotarias();
    }
}