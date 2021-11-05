using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Contratos.Models.ActaNotarial;

namespace PortalAdministrador.Services
{
    public interface IActaNotarialService
    {
        
        Task<ActaNotarialModel> ObtenerActaNotarial(long TramiteId);

        Task<FirmaActaNotarialModel> FirmarActaNotarial(string pin,long TramiteId);
        Task<TramiteRechazadoReturnModel> RechazarTramiteNotarial(TramiteRechazadoModel tramite);
        Task<List<AutorizacionTramitesResponse>> FirmaActaNotarialLote(AutorizacionTramitesRequest request);
        Task<ActaResumen> ObtenerResumen(long tramiteId);
        Task<bool> CrearActaParaFirmaManual(ActaCreate acta);
    }
}
