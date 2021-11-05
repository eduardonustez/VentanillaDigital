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
    public class TipoIdentificacionServicio : BaseServicio, ITipoIdentificacionServicio
    {
        private ITipoIdentificacionRepositorio _tipoIdentificacionRepositorio { get; }

        public TipoIdentificacionServicio(ITipoIdentificacionRepositorio tipoIdentificacionRepositorio) : base(tipoIdentificacionRepositorio)
        {
            _tipoIdentificacionRepositorio = tipoIdentificacionRepositorio;
        }
        public async Task<IEnumerable<TipoIdentificacionReturnDTO>> ObtenerTiposIdentificacion()
        {
            var listado = await _tipoIdentificacionRepositorio.ObtenerTodoAsync();
            return listado.ToList().Select(item => item.Adaptar<TipoIdentificacionReturnDTO>()).ToList();
        }

        public async Task<TipoIdentificacionReturnDTO> ObtenerTipoIdentificacionPorId(int TipoIdentificacionId)
        {
            bool existeTipoIdentificacion = (await _tipoIdentificacionRepositorio.Obtener(x => x.IsDeleted == false && x.TipoIdentificacionId == TipoIdentificacionId).ConfigureAwait(false)).Any();
            if (existeTipoIdentificacion)
            {
                TipoIdentificacion tipoIdentificacion = (await _tipoIdentificacionRepositorio.Obtener(x => x.IsDeleted == false && x.TipoIdentificacionId == TipoIdentificacionId).ConfigureAwait(false)).FirstOrDefault();
                return tipoIdentificacion?.Adaptar<TipoIdentificacionReturnDTO>();
            }
            return null;
        }
    }
}
