using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class ExcepcionHuella:EntidadBase
    {
        public long ExcepcionHuellaId { get; set; }
        public string Descripcion { get; set; }
        public int DedosExceptuados { get; set; }
        public long ComparecienteId { get; set; }
        public virtual Compareciente Compareciente { get; set; }
    }
}
