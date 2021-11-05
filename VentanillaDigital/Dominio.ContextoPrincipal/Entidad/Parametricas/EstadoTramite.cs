using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class EstadoTramite:EntidadBase
    {
        public long EstadoTramiteId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
