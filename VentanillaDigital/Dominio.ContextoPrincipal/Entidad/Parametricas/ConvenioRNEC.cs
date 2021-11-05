using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class ConvenioRNEC:EntidadBase
    {
        public long ConvenioRNECId { get; set; }
        public long NotariaId { get; set; }
        public string Convenio { get; set; }
        public long IdCliente { get; set; }
        public long IdZona { get; set; }
        public long IdRol { get; set; }
        public long IdOficina { get; set; }
        public virtual Notaria Notaria { get; set; }
    }
}
