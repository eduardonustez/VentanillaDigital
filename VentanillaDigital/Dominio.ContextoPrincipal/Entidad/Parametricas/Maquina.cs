using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Maquina:EntidadBase
    {
        public long MaquinaId { get; set; }
        public string Nombre { get; set; }
        public string MAC { get; set; }
        public string DireccionIP { get; set; }
        public int TipoMaquina { get; set; }
        public long NotariaId { get; set; }
        public string CanalWacom { get; set; }
        public string EstadoWacomSigCaptX { get; set; }
        public string EstadoDllWacom { get; set; }
        public string EstadoCaptor { get; set; }
        public virtual Notaria Notaria { get; set; }
    }
}
