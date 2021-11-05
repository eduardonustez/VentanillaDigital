using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ITipoTramiteServicio:IDisposable
    {
        Task<IEnumerable<TipoTramiteReturnDTO>> ObtenerTiposTramites();
    }
}
