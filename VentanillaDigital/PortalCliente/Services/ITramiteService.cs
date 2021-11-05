using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using System.Threading;

namespace PortalCliente.Services
{
    public interface ITramiteService
    {

        Task<Data.Tramite> ObtenerTramite(long tramiteId);
        Task<TramitePendienteAutorizacionModel> ObtenerPendientesAutorizacion(DefinicionFiltro definicionFiltroSimple, CancellationToken cancellationToken =  default(System.Threading.CancellationToken));

        Task<bool> CrearCompareciente(long TramiteId, Compareciente compareciente);
        Task<long> CrearTramite(Data.Tramite tramite);
        Task<bool> ActualizarTramite(Data.Tramite tramite);

        Task<TramitePendienteAutorizacionModel> ObtenerTramitesEnProceso(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        Task<TramitePendienteAutorizacionModel> ObtenerTramitesAutorizados(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        Task<TramitePendienteAutorizacionModel> ObtenerTramitesPendientes(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        Task<TramitePendienteAutorizacionModel> ObtenerTramitesRechazados(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }
}
