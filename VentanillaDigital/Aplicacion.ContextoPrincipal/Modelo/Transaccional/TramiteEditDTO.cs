using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteEditDTO: EditRegisterDTO
    {
        [Required]
        public long TramiteId { get; set; }
        public int CantidadComparecientes { get; set; }
        public long TipoTramiteId { get; set; }
        public string DatosAdicionales { get; set; }
    }
}
