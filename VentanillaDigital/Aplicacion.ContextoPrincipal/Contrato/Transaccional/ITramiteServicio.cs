using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ITramiteServicio : IDisposable
    {
        Task<TramiteReturnDTO> ObtenerTramite(long tramiteId);
        Task<TramiteDetalleDTO> ObtenerTramiteDetalle(long tramiteId);
        Task<TramiteReturnDTO> CrearTramite(TramiteCreateDTO tramite);
        Task<bool> ActualizarTramite(TramiteEditDTO tramite);
        Task<bool> ActualizarEstadoTramite(long tramiteId);
        Task AutorizarTramite(long tramiteId);
        Task<ListaTramitePendienteReturnDTO> ObtenerTramitesPendientes(DefinicionFiltro definicionFiltro);
        Task<IEnumerable<TramiteInfoBasica>> ObtenerTramitesPorNumeroIdentificacion(string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<HistorialTramite>> ObtenerHistorialTramite(long tramiteId);
        Task<PaginableResponse<TramiteInfoBasica>> ObtenerTramitesPorNumeroIdentificacionPaginado(DefinicionFiltro filtro);
        Task<ListaTramitePendienteReturnDTO> ObtenerTramitesPendientesAutPaginado(FiltroTramites filtroTramites);
        Task<ListaTramitePendienteReturnDTO> ObtenerTramitesAutorizadoPaginado(FiltroTramites filtroTramites);
        Task<ListaTramitePendienteReturnDTO> ObtenerTramitesEnProcesoPaginado(FiltroTramites filtroTramites);
        Task<ListaTramitePendienteReturnDTO> ObtenerTramitesRechazadosPaginado(FiltroTramites filtroTramites);
    }
}
