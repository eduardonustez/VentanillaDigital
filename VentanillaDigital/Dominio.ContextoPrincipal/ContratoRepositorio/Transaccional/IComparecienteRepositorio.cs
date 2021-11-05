using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional
{
    public interface IComparecienteRepositorio : IRepositorioBase<Compareciente>, IDisposable
    {
        Task<List<Compareciente>> ObtenerComparecientesPorTramiteID(long tramiteId);

        Persona ObtenerPersona(long? TipoIdentificacionId, string NumeroDocumento, bool? esEliminado);

        int ActualizarPersona(Persona PersonaUpdate);

        int ActualizarEstadoTramite(Tramite tramite);
        Task<EstadoTramite> ObtenerEstadoTramite(string nombreEstado);
    }
}
