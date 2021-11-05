using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class TipoIdentificacion:EntidadBase
    {
        public int TipoIdentificacionId { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
    }
}
