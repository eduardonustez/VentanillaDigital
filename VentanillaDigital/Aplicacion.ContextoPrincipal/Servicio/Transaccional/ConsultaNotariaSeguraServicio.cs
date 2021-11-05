using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public class ConsultaNotariaSeguraServicio : BaseServicio, IConsultaNotariaSeguraServicio
    {
        private IConsultaNotariaSeguraRepositorio _consultaNotariaSegura;

        public ConsultaNotariaSeguraServicio(IConsultaNotariaSeguraRepositorio consultaNotariaSegura): base(consultaNotariaSegura)
        {
            _consultaNotariaSegura = consultaNotariaSegura;
        }

        public async Task<string> InsertarConsultaNotariaSegura(ConsultaNotariaSeguraInsertDTO consultaNotariaSeguraInsert)
        {
            try
            {
                _consultaNotariaSegura.Agregar(new ConsultaNotariaSegura
                {
                    TramiteId = consultaNotariaSeguraInsert.Nut,
                    TramiteIdHash = consultaNotariaSeguraInsert.NutHash,
                    NotariaId = consultaNotariaSeguraInsert.NotariaId,
                    Email = consultaNotariaSeguraInsert.Email,
                    FechaConsulta = consultaNotariaSeguraInsert.FechaConsulta,
                    EncontroArchivo = consultaNotariaSeguraInsert.SeEncontroArchivo
                });
                _consultaNotariaSegura.UnidadDeTrabajo
                    .Commit();
            }
            catch { }

            return "ok";
        }
    }
}