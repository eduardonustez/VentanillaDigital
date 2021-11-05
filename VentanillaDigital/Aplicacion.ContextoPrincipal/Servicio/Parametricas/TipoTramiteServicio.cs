using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericExtensions;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class TipoTramiteServicio : BaseServicio, ITipoTramiteServicio
    {
        private ITipoTramiteRepositorio _tipoTramiteRepositorio { get; }

        public TipoTramiteServicio(ITipoTramiteRepositorio tipoTramiteRepositorio) :base(tipoTramiteRepositorio)
        {
            _tipoTramiteRepositorio = tipoTramiteRepositorio;
        }
        public async Task<IEnumerable<TipoTramiteReturnDTO>> ObtenerTiposTramites()
        {
            var listado = await _tipoTramiteRepositorio.Obtener(t=>t.IsDeleted==false);
            return listado.ToList().Select(item => item.Adaptar<TipoTramiteReturnDTO>()).ToList();
        }
    }
}
