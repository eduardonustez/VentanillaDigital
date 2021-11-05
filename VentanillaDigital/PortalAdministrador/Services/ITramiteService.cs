using ApiGateway.Contratos.Models;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public interface ITramiteService
    {
        Task<Data.TramiteModel> ConsultarTramitePorId(long tramiteId);
        Task<IEnumerable<Data.DatosComparecienteModel>> ConsultarComparecientesPorTramiteId(long tramiteId);
        Task<IEnumerable<TramiteInfoBasica>> ConsultarTramitesPorNumeroIdentificacion(string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin);
        Task<PaginableResponse<TramiteInfoBasica>> ConsultarTramitesPorNumeroIdentificacionPaginado(DefinicionFiltro filtro);
        Task<IEnumerable<HistorialTramite>> ConsultarHistorialTramite(long tramiteId);
    }
}
