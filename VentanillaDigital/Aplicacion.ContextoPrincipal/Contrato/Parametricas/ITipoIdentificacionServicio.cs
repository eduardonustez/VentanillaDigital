using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ITipoIdentificacionServicio:IDisposable
    {
        Task<IEnumerable<TipoIdentificacionReturnDTO>> ObtenerTiposIdentificacion();
        Task<TipoIdentificacionReturnDTO> ObtenerTipoIdentificacionPorId(int TipoIdentificacionId);
    }
}
