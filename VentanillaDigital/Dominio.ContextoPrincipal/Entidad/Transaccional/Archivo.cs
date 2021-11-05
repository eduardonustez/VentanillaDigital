using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class Archivo:EntidadBase
    {
        public long ArchivoId { get; set; }
        public long MetadataArchivoId { get; set; }
        public string Contenido { get; set; }
        public virtual MetadataArchivo MetadataArchivo { get; set; }
    }
}
