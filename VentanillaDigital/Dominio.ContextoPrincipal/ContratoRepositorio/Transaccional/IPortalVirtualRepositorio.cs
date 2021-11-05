using Dominio.ContextoPrincipal.Entidad;
using Dominio.Nucleo;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IPortalVirtualRepositorio : IRepositorioBase<TramitesPortalVirtual>, IDisposable
    {
        Task<RespuestaProcedimientoViewModel> ObtenerTramitePortalVirtual(DefinicionFiltro definicionFiltro);
    }
}
