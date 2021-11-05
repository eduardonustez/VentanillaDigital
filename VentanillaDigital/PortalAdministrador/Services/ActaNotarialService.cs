using ApiGateway.Contratos.Models.ActaNotarial;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Models.Transaccional;
using Blazored.SessionStorage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public class ActaNotarialService : IActaNotarialService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public ActaNotarialService(ISessionStorageService sessionStorageService, ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }

        public async Task<bool> CrearActaParaFirmaManual(ActaCreate acta)
        {
            var resultado = await _customHttpClient.PostJsonAsync<bool>("ActaNotarial/CrearActaParaFirmaManual", acta);
            return resultado;
        }

        public async Task<List<AutorizacionTramitesResponse>> FirmaActaNotarialLote(AutorizacionTramitesRequest request)
        {
            var resultado = await _customHttpClient.PostJsonAsync<List<AutorizacionTramitesResponse>>(
                $"/api/ActaNotarial/FirmaActaNotarialLote/",
                request
                );
            return resultado;
        }

        public async Task<FirmaActaNotarialModel> FirmarActaNotarial(string pin, long tramiteId)
        {
            var resultado = await _customHttpClient.PostJsonAsync<FirmaActaNotarialModel>($"/api/ActaNotarial/FirmarActaNotarial", new PinFirmaModel() { Pin = pin, TramiteId = tramiteId });
            return resultado;
        }

        public async Task<ActaNotarialModel> ObtenerActaNotarial(long TramiteId)
        {
            var resultado = await _customHttpClient.GetJsonAsync<ActaNotarialModel>($"/api/ActaNotarial/ObtenerActaNotarial/{TramiteId}");
            return resultado;
        }

        public async Task<ActaResumen> ObtenerResumen(long tramiteId)
        {
            var resultado = await _customHttpClient.GetJsonAsync<ActaResumen>($"/api/ActaNotarial/ObtenerResumen/{tramiteId}");
            return resultado;
        }

        public async Task<TramiteRechazadoReturnModel> RechazarTramiteNotarial(TramiteRechazadoModel tramite)
        {
            var resultado = await _customHttpClient.PostJsonAsync<TramiteRechazadoReturnModel>($"/api/ActaNotarial/RechazarTramiteNotarial", tramite);
            return resultado;
        }
        
    }

}
