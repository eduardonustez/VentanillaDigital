using System.Collections.Generic;
using Aplicacion.Nucleo.Base;
using System.Threading.Tasks;
using System.Linq;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using GenericExtensions;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class UbicacionServicio : BaseServicio, IUbicacionServicio
    {
        private IUbicacionRepositorio _ubicacionRepositorio { get; }
     

        public UbicacionServicio(
            IUbicacionRepositorio ubicacionRepositorio
            ) :base(ubicacionRepositorio)
        {
            
            _ubicacionRepositorio = ubicacionRepositorio;
        }

        public async Task<IEnumerable<UbicacionReturnDTO>> GetDepartamentos()
        {
            var departamentos = (await _ubicacionRepositorio.Obtener(u=>u.UbicacionPadreId==null)).ToList();
            return departamentos.Select(p=>p.Adaptar<UbicacionReturnDTO>()).ToList();
        }

        public async Task<IEnumerable<UbicacionReturnDTO>> GetCiudades(int departamentoId)
        {
            var ciudades = (await _ubicacionRepositorio.Obtener(u => u.UbicacionPadreId == departamentoId)).ToList();
            return ciudades.Select(p => p.Adaptar<UbicacionReturnDTO>()).ToList();
        }

        
    }
}
