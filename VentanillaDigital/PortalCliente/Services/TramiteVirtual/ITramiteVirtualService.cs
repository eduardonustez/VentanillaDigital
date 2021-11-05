using ApiGateway.Contratos.Models.NotariaVirtual;
using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public interface ITramiteVirtualService
    {
        Task<TramitesVirtualesModel> ObtenerTramitesVirtuales(DefinicionFiltro definicionFiltroSimple, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> ValidarConvenioNotariaVirtual(long NotariaId);
        Task<TramiteVirtualModel> ConsultarTramiteVirtualPorId(long tramiteId);
        Task<ArchivoTramiteVirtual> ConsultarArchivoTramiteVirtualPorId(long archivoId, string AuthToken);
        Task<TramiteVirtualModel> CambiarEstado(
            long tramiteId, 
            int estado, 
            decimal precio, 
            string razon, 
            List<UploadFileModel> files, 
            string ClaveTestamento, 
            Coordenadas coordenadas,
            int? actoPrincipalId,
            List<ActoPorTramiteModel> actosNotariales,
            string datosAdicionales);
        Task<IEnumerable<TramiteVirtualMensajeModel>> ConsultarMensajesTramiteVirtual(long tramitePortalVirtualId);
        Task<TramiteVirtualModel> CambiarEstadoCliente(
            long tramiteId,
            List<UploadFileModel> files,
            bool estado,
            string mensaje,
            string usuario);
        Task<IEnumerable<RecaudoTramiteModel>> ConsultarRecaudosTramite(long tramitePortalVirtualId);
        Task<TestamentoModel> DescargarTestamento(long tramiteId, string claveTestamento);
        Task<ResponseCrearRecaudo> GuardarRecaudo(CrearRecaudoTramiteVirtualModel model);
        Task<IEnumerable<ActoNotarialModel>> ObtenerActosNotariales();
        Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId);
        Task<IEnumerable<EstadoTramitesVirtualesResponse>> ObtenerEstadosTramite(bool IsDeleted);
        Task<bool> EliminarRecaudo(long recaudoTramiteVirtualId);
        Task<bool> EnviarRecaudo(List<long> archivosPortalVirtualId, List<UploadFileModel> uploadFileModel, long recaudoTramiteVirtualId);
        Task<decimal> ObtenerTotalPagadoTramite(int tramitePortalVirtualId);
        Task<UrlMiFirmaModel> ConsultarUrlSubirArchivosMiFirma();
        Task<MiFirmaModel> ObtenerInformacionMiFirma(long notariaId);
    }
}
