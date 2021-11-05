using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad
{
    public class ArchivosPortalVirtual : EntidadBase
    {
        public long ArchivosPortalVirtualId { get; set; }
        public int TramitesPortalVirtualId { get; set; }
        public short TipoArchivo { get; set; }
        public string Formato { get; set; }
        public string Base64 { get; set; }
        public string Nombre { get; set; }
        public virtual TramitesPortalVirtual TramitesPortalVirtual { get; set; }
        public virtual TipoArchivoTramiteVirtual TipoArchivoTramiteVirtual { get; set; }
    }
}
