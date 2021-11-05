using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ArchivosComparecientesDTO 
    {
        public int TipoArchivo { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamanio { get; set; }
        public string Ruta { get; set; }
        public string Contenido { get; set; }
    }
}
