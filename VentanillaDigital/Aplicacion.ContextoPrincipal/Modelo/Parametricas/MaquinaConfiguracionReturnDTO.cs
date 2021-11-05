using System;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class MaquinaConfiguracionReturnDTO
    {
        public DateTime FechaModificacion { get; set; }
        public string CorreoUsuario { get; set; }
        public string DireccionIP { get; set; }
        public string MAC { get; set; }
        public long NotariaId { get; set; }
        public string CanalWacom { get; set; }
        public string EstadoWacomSigCaptX { get; set; }
        public string EstadoDllWacom { get; set; }
        public string EstadoCaptor { get; set; }
    }
}
