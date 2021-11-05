using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Transaccional
{
    public interface IComparecienteServicio : IDisposable
    {
        Task<ComparecienteCreateReturnDTO> AgregarCompareciente(ComparecienteCreateDTO compareciente);
        Task<IEnumerable<TramiteComparecienteReturnDTO>> ObtenerComparecientesPorTramiteID(long tramiteId);
        Task<IEnumerable<ComparecienteDatosDTO>> ObtenerDatosComparecientesPorTramiteId(long tramiteId);
    }
}
