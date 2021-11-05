using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Parametricas
{
    public interface IPortalVirtualServicio: IDisposable
    {
        Task<bool> RegistrarCiudadano(TramitePortalVirtualCiudadanoDTO Notario);
        Task<ActualizarTramiteVirtualResponseDTO> ActualizarTramiteVirtual(ActualizarTramiteVirtualDTO actualizarTramite);
        Task<ListaTramitePortalVirtualReturnDTO> ObtenerTramitePortalVirtual(DefinicionFiltro definicionFiltro);
        Task<TramiteVirtualModel> ConsultarTramiteVirtualPorId(ConsultarTramiteDTO consultarTramite);
        Task<ArchivoTramiteVirtual> ConsultarArchivoTramiteVirtualPorId(long tramiteId);
        Task<TramiteVirtualModel> CambiarEstado(long tramiteId, CambiarEstadoTramiteVirtualModel body);
        Task<ValidaTramiteResponseDTO> ValidarTramitePersona(ValidarTramitePersonaDTO validarTramitePersona);
        Task<TestamentoModel> ObtenerTestamento(long tramiteId, string claveTestamento);
        Task<TramiteVirtualModel> CambiarEstadoCliente(long tramiteId, List<UploadFileModel> files, bool estado, string usuario, string mensaje);
        Task<IEnumerable<TramiteVirtualMensajeModel>> ConsultarMensajesTramiteVirtual(long archivoId);
        Task<ResponseCrearRecaudo> GuardarRecaudo(CrearRecaudoTramiteVirtualModel body);
        Task<IEnumerable<RecaudoTramiteModel>> ConsultarRecaudosTramite(long tramiteId);
        Task<bool> EliminarRecaudo(long recaudoTramiteVirtualId);
        Task<bool> ActualizarPago(long recaudoTramiteVirtualId, ActualizarPagoModel model);
        Task<bool> EnviarRecaudo(long recaudoTramiteVirtualId, EnviarRecaudoModel model);
        Task<decimal> TotalPagadoTramite(int tramitePortalVirtualId);
        Task<string> ConsultarUrlSubirArchivosMiFirma(long notariaId);
    }
}
