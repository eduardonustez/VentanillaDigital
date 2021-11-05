using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas
{
    public interface IActoNotarialRepositorio : IRepositorioBase<ActoNotarial>, IDisposable
    {
        Task<IEnumerable<ActoNotarial>> ObtenerTodosActosNotariales();
        Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId);
        Task<int> ObtenerActoNotarialId(string codigo);
    }
}
