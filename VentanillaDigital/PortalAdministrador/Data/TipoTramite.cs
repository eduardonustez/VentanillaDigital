using System;

namespace PortalAdministrador.Data
{
    public class TipoTramite
    {
        public long TipoTramiteId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public long CodigoTramite { get; set; }
        public long ProductoReconoserId { get; set; }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
