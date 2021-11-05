using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class MaquinaCreateDTO : ModeloDTO
    {
        public string Nombre { get; set; }
        public string MAC { get; set; }
        public string DireccionIP { get; set; }
        public int TipoMaquina { get; set; }
        public long NotariaId { get; set; }
        public string UserName { get; set; }
        public string CanalWacom { get; set; }
        public string EstadoWacomSigCaptX { get; set; }
        public string EstadoDllWacom { get; set; }
        public string EstadoCaptor { get; set; }
    }
}
