using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using System.Threading.Tasks;
using GenericExtensions;
using System.Linq;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class NotariasUsuarioServicio : BaseServicio, INotariasUsuarioServicio
    {
        public INotariasUsuarioRepositorio _notariasUsuarioRepositorio { get; }
        public NotariasUsuarioServicio(INotariasUsuarioRepositorio notariasUsuarioRepositorio) : base(notariasUsuarioRepositorio)
        {
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
        }

        public async Task<int> CrearNotariaUsuario(NotariaUsuarioCreateDTO notaria)
        {
            var usuarioExistente = (await _notariasUsuarioRepositorio.Obtener(x => x.IsDeleted == false && x.UsuariosId == notaria.UsuarioId).ConfigureAwait(false)).Any();
            if (!usuarioExistente)
            {
                var notariaUsuario = notaria.Adaptar<NotariaUsuarios>();
                _notariasUsuarioRepositorio.Agregar(notariaUsuario);
                int usuarioNotariaAsociado = await _notariasUsuarioRepositorio.UnidadDeTrabajo.CommitAsync();
                if (usuarioNotariaAsociado > 0)
                {
                    return usuarioNotariaAsociado;
                }
            }
            return 0;
        }

        public async Task<ContactoFuncionarioReturnDTO> ObtenerDatosDeContacto(string usuarioId)
        {
            var notariaUsuario =
                (await _notariasUsuarioRepositorio.Obtener(u => u.UsuariosId == usuarioId))
                .FirstOrDefault();


            var ret = new ContactoFuncionarioReturnDTO()
            {
                UsuarioId = usuarioId,
                Celular = notariaUsuario?.Celular,
                Correo = notariaUsuario?.UserEmail,
            };
            return ret;
        }
    }
}
