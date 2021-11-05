using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal.Models;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional
{
    public interface ITramiteRepositorio : IRepositorioBase<Tramite>, IDisposable
    {
        Task<bool> NotariaActiva(long NotariaId);
        Task<bool> TipoTramiteActivo(long TipoTramiteId);
        Task<EstadoTramite> ObtenerEstadoTramite(string Nombre);
        Task<RespuestaProcedimientoViewModel> ObtenerTramitesPaginado(DefinicionFiltro definicionFiltro);

        Task<Tramite> ObtenerTramiteActa(long tramiteId, string tipoActa);

        Task<IEnumerable<Tramite>> ObtenerTramitesActa(IEnumerable<long> tramitesId, string tipoActa);
        Task<DatosTramiteResumen> ObtenerDatosTramiteResumen(long tramiteId);

        Task<RespuestaProcedimientoViewModel> ObtenerTramitesPendientesAutPaginado(FiltroTramites filtroTramites);
        Task<RespuestaProcedimientoViewModel> ObtenerTramitesAutorizadoPaginado(FiltroTramites filtroTramites);
        Task<RespuestaProcedimientoViewModel> ObtenerTramitesEnProcesoPaginado(FiltroTramites filtroTramites);
        Task<RespuestaProcedimientoViewModel> ObtenerTramitesRechazadosPaginado(FiltroTramites filtroTramites);
    }
}
