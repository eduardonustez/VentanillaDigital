using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Parametricas
{
    public interface IActoNotarialServicio : IDisposable
    {
        Task<IEnumerable<ActoNotarial>> ObtenerTodosActosNotariales();
        Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId);
    }
}
