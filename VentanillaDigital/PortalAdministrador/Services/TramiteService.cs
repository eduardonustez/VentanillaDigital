using ApiGateway.Contratos.Models;
using Blazored.SessionStorage;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public class TramiteService : ITramiteService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public TramiteService(ISessionStorageService sessionStorageService, ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }

        public async Task<Data.TramiteModel> ConsultarTramitePorId(long tramiteId)
        {
            var res = await _customHttpClient.GetJsonAsync<Data.TramiteModel>($"/api/Tramite/ObtenerTramite/{tramiteId}");
            return res;
        }

        public async Task<IEnumerable<Data.DatosComparecienteModel>> ConsultarComparecientesPorTramiteId(long tramiteId)
        {
            var res = await _customHttpClient.GetJsonAsync<IEnumerable<Data.DatosComparecienteModel>>($"/api/Compareciente/ObtenerTodoPorTramiteId/{tramiteId}");
            return res;
        }

        public async Task<IEnumerable<ApiGateway.Contratos.Models.TramiteInfoBasica>> ConsultarTramitesPorNumeroIdentificacion(string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin)
        {
            var res = await _customHttpClient.GetJsonAsync<IEnumerable<TramiteInfoBasica>>($"/api/Tramite/ObtenerTramitesPorNumeroIdentificacion/{numeroIdentificacion}/{fechaInicio.ToString("yyyy-MM-dd")}/{fechaFin.ToString("yyyy-MM-dd")}");
            return res;
        }

        public async Task<IEnumerable<ApiGateway.Contratos.Models.HistorialTramite>> ConsultarHistorialTramite(long tramiteId)
        {
            var res = await _customHttpClient.GetJsonAsync<IEnumerable<ApiGateway.Contratos.Models.HistorialTramite>>($"/api/Tramite/ObtenerHistorialTramite/{tramiteId}");
            return res;
        }

        public async Task<PaginableResponse<TramiteInfoBasica>> ConsultarTramitesPorNumeroIdentificacionPaginado(DefinicionFiltro filtro)
        {
            var res = await _customHttpClient.PostJsonAsync<PaginableResponse<TramiteInfoBasica>>($"/api/Tramite/ObtenerTramitesPorNumeroIdentificacionPaginado", filtro);
            return res;
        }
    }
}
