using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using System;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Transaccional
{
    public interface IActaNotarialServicio : IDisposable
    {
        Task<ActaNotarialReturnDTO> ObtenerActaNotarial(ActaNotarialGetDTO actaNotarialGetDTO);
        Task<string> ObtenerActaNotarialPublico(string codigo);
        Task<string> ObtenerActaNotarialSegura(ActaNotarialSeguraRequest codigo);
        Task<string> ObtenerActaNotarialSeguraHistorico(ActaNotarialSeguraRequest codigo);
        Task<TramiteRechazadoReturnDTO> RechazarTramiteNotarial(TramiteRechazadoDTO pinFirmaDTO);
        Task<TramiteRechazadoReturnDTO> CancelarTramiteNotarial(TramiteRechazadoDTO pinFirmaDTO);
        Task<AutorizacionTramitesResponseDTO[]> FirmaActaNotarialLote(AutorizacionTramitesDTO autorizacionTramites);
        Task<ActaResumen> ObtenerResumen(long tramiteId);
        Task CrearActaParaFirmaManual(ActaCreateDTO actaCreate);
    }
}