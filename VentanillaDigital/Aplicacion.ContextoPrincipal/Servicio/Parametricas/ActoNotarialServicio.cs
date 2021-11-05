using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Infraestructura.Transversal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Parametricas
{
    public class ActoNotarialServicio : BaseServicio, IActoNotarialServicio
    {
        private readonly IActoNotarialRepositorio _actoNotarialRepositorio;

        public ActoNotarialServicio(IActoNotarialRepositorio actoNotarialRepositorio)
            : base(actoNotarialRepositorio)
        {
            _actoNotarialRepositorio = actoNotarialRepositorio;
        }

        public Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId)
            => _actoNotarialRepositorio.ObtenerActosPorTramite(tramiteId);

        public Task<IEnumerable<ActoNotarial>> ObtenerTodosActosNotariales()
            => _actoNotarialRepositorio.ObtenerTodosActosNotariales();
    }
}
