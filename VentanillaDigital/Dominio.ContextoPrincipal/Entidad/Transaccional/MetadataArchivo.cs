using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class MetadataArchivo:EntidadBase
    {
        public long MetadataArchivoId { get; set; }

        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamanio { get; set; }

        public string Ruta { get; set; }
        public virtual Archivo Archivo { get; set; }
    }
}
