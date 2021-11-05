using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Log
{
    public class LogTramitePortalVirtual : EntidadBase
    {
        public int LogTramiteVirtualPortalId { get; set; }
        public int TramitePortalVirtualId { get; set; }
        public string ClaveTestamentoCerrado { get; set; }
        public bool EnvioSNR { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public int EstadoTramiteVirtualId { get; set; }

        public string LogResponseSNR { get; set; }
    }
}
