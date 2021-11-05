using Aplicacion.ContextoPrincipal.Enums;
using Infraestructura.Transversal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ListaTramitePendienteReturnDTO:RespuestaServicioViewModel
    {
        public IEnumerable<TramitePendienteReturnDTO> Pendientes { get; set; }
    }
    public class TramitePendienteReturnDTO
    {
        public long Id { get; set; }
        public string TipoTramite { get; set; }
        public DateTime Fecha { get; set; }
        public int Comparecientes { get; set; }
        public string DocCompareciente { get; set; }
        public string Estado { get; set; }
        public string UsuarioOperador { get; set; }
        public string NombresOperador { get; set; }
        public string ApellidosOperador { get; set; }
        public string DatosAdicionales { get; set; }
        public string NombreOperador 
        {
            get { return $"{NombresOperador} {ApellidosOperador}"; }
            private set { }
        }
        public string MotivoRechazo {
            get
            {
                if (DatosAdicionales != null)
                    return JsonConvert.DeserializeObject<TramiteRechazadoDTO>(DatosAdicionales)?.MotivoRechazo;
                else return "";
            }
            private set{}
        }
            

    }
    
}
