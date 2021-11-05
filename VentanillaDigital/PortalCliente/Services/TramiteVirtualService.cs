using ApiGateway.Contratos.Models.NotariaVirtual;
using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public class TramiteVirtualService : ITramiteVirtualService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public TramiteVirtualService(ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }

        public async Task<UrlMiFirmaModel> ConsultarUrlSubirArchivosMiFirma()
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<UrlMiFirmaModel>($"/api/TramitesPortalVirtual/ConsultarUrlSubirArchivosMiFirma");
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"ConsultarUrlSubirArchivosMiFirma: {ex.Message}");
                throw ex;
            }
        }

        public async Task<TramiteVirtualModel> CambiarEstado(
            long tramiteId,
            int estado,
            decimal precio,
            string razon,
            List<UploadFileModel> files,
            string ClaveTestamento,
            Coordenadas coordenadas,
            int? actoPrincipalId,
            List<ActoPorTramiteModel> actosNotariales,
            string datosAdicionales)
        {
            try
            {
                var res = await _customHttpClient.PostJsonAsync<TramiteVirtualModel>($"/api/TramitesPortalVirtual/CambiarEstado/{tramiteId}",
                    new CambiarEstadoTramiteVirtualModel
                    {
                        Estado = estado,
                        Precio = precio,
                        Razon = razon,
                        Files = files,
                        ActoPrincipalId = actoPrincipalId,
                        ClaveTestamento = ClaveTestamento,
                        Coordenadas = coordenadas,
                        ActosNotariales = actosNotariales,
                        DatosAdicionalesCierre = datosAdicionales
                    });

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                throw ex;
            }
        }

        public async Task<bool> EliminarRecaudo(long recaudoTramiteVirtualId)
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<bool>($"/api/TramitesPortalVirtual/EliminarRecaudo/{recaudoTramiteVirtualId}");
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                throw ex;
            }
        }

        public async Task<bool> EnviarRecaudo(List<long> archivosPortalVirtualId, List<UploadFileModel> uploadFileModel, long recaudoTramiteVirtualId)
        {
            try
            {
                var res = await _customHttpClient.PostJsonAsync<bool>($"/api/TramitesPortalVirtual/EnviarRecaudo/{recaudoTramiteVirtualId}",
                    new EnviarRecaudoModel
                    {
                        ArchivosPortalVirtualId = archivosPortalVirtualId,
                        Archivos = uploadFileModel?.Select(uploadFileModel => new ArchivoRecaudoModel
                        {
                            Data = uploadFileModel.Data,
                            Formato = uploadFileModel.Formato,
                            Index = uploadFileModel.Index,
                            Nombre = uploadFileModel.Nombre,
                            Type = uploadFileModel.Type
                        })?.ToList()
                    });

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                throw ex;
            }
        }

        public async Task<TramiteVirtualModel> CambiarEstadoCliente(long tramiteId, List<UploadFileModel> files, bool estado, string mensaje, string usuario)
        {
            try
            {
                var res = await _customHttpClient.PostJsonAsync<TramiteVirtualModel>($"/api/TramitesPortalVirtual/CambiarEstadoCliente/{tramiteId}",
                    new CambiarEstadoTramiteVirtualClienteModel
                    {
                        Usuario = usuario,
                        Estado = estado,
                        Mensaje = mensaje,
                        Files = files
                    });

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ArchivoTramiteVirtual> ConsultarArchivoTramiteVirtualPorId(long archivoId, string AuthToken)
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<ArchivoTramiteVirtual>($"/api/TramitesPortalVirtual/ConsultarArchivoTramiteVirtualPorId/{archivoId}/?token={AuthToken}");
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<TramiteVirtualMensajeModel>> ConsultarMensajesTramiteVirtual(long tramitePortalVirtualId)
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<IEnumerable<TramiteVirtualMensajeModel>>($"/api/TramitesPortalVirtual/ConsultarMensajesTramiteVirtual/{tramitePortalVirtualId}");

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<RecaudoTramiteModel>> ConsultarRecaudosTramite(long tramitePortalVirtualId)
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<IEnumerable<RecaudoTramiteModel>>($"/api/TramitesPortalVirtual/ConsultarRecaudosTramite/{tramitePortalVirtualId}");
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"ConsultarRecaudosTramite: {ex.Message}");
                throw ex;
            }
        }

        public async Task<TramiteVirtualModel> ConsultarTramiteVirtualPorId(long tramiteId)
        {
            try
            {
                ConsultaTramiteRequest request = new ConsultaTramiteRequest { TramiteId = tramiteId };
                var res = await _customHttpClient.PostJsonAsync<TramiteVirtualModel>($"/api/TramitesPortalVirtual/ConsultarPorId/", request);
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener tramite portal virtual por id: {ex.Message}");
                return null;
            }
        }

        public async Task<TestamentoModel> DescargarTestamento(long tramiteId, string claveTestamento)
        {
            try
            {
                var resultado = await _customHttpClient.GetJsonAsync<TestamentoModel>($"/api/TramitesPortalVirtual/ObtenerTestamento/{tramiteId}/{claveTestamento}");
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> ObtenerTotalPagadoTramite(int tramitePortalVirtualId)
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<decimal>($"/api/TramitesPortalVirtual/obtenerTotalPagadoTramite/{tramitePortalVirtualId}");

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"ObtenerTotalPagadoTramite: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ResponseCrearRecaudo> GuardarRecaudo(CrearRecaudoTramiteVirtualModel model)
        {
            try
            {
                var res = await _customHttpClient.PostJsonAsync<ResponseCrearRecaudo>($"/api/TramitesPortalVirtual/GuardarRecaudo", model);

                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Crear Recaudo: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<ActoNotarialModel>> ObtenerActosNotariales()
        {
            try
            {
                var res = await _customHttpClient.GetJsonAsync<IEnumerable<ActoNotarialModel>>($"/api/TramitesPortalVirtual/ObtenerActosNotariales");
                return res;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener actos notariales: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId)
        {
            try
            {
                var resultado = await _customHttpClient.GetJsonAsync<IEnumerable<ActoPorTramiteModel>>($"/api/TramitesPortalVirtual/ObtenerActosPorTramite/{tramiteId}");
                return resultado;
            }
            catch (Exception ex)
            {
                Console.Write($"Error ObtenerActosPorTramite: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<EstadoTramitesVirtualesResponse>> ObtenerEstadosTramite(bool IsDeleted)
        {
            try
            {
                var resultado = await _customHttpClient.GetJsonAsync<List<EstadoTramitesVirtualesResponse>>($"/api/NotariaVirtual/EstadosTramiteVirtual/{IsDeleted}");
                Console.Write($"estados: 😁😁 {resultado.Count}");
                return resultado;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al obtener lista de estados de tramite: {ex.Message}");
                return null;
            }
        }

        public async Task<TramitesVirtualesModel> ObtenerTramitesVirtuales(DefinicionFiltro definicionFiltro, CancellationToken cancellationToken = default)
        {
            try
            {
                var resultado = await _customHttpClient.PostJsonAsync<TramitesVirtualesModel>("/api/TramitesPortalVirtual/ObtenerTramitePortalVirtual", definicionFiltro, cancellationToken);
                return resultado;
            }
            catch (Exception ex)
            {
                Console.Write($"Error al descargar el testamento: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ValidarConvenioNotariaVirtual(long NotariaId)
        {
            try
            {
                var resultado = await _customHttpClient.GetJsonAsync<ConvenioNotariaVirtualResponse>($"/api/NotariaVirtual/ConvenioNotariaVirtual/{NotariaId}");
                Console.Write($"Valor 😁😁 {resultado}");
                return resultado.esNotariaVirtual;
            }
            catch (Exception ex)
            {
                Console.Write($"Valor Error al validar Convenio: {ex.Message}, {ex.StackTrace}");
                return false;
            }
        }

        public async Task<MiFirmaModel> ObtenerInformacionMiFirma(long notariaId)
        {
            try
            {
                var configuracionMiFirma = await _customHttpClient.PostJsonAsync<MiFirmaModel>($"/api/NotariaVirtual/ObtenerMiConfiguracionMiFirma/{notariaId}", null);
                return configuracionMiFirma;
            }
            catch (Exception ex)
            {
                Console.Write($"ObtenerInformacioMiFirma: {ex.Message}");
                return null;
            }
        }
    }
}
