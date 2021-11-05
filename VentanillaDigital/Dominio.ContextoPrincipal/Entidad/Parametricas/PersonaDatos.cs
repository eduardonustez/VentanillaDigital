using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class PersonaDatos : EntidadBase
    {
        public long PersonaDatosId { get; set; }
        public long PersonaId { get; set; }
        public TipoDatoId TipoDatoId { get; set; }
        public string ValorDato { get; set; }
        public TipoDato TipoDato { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
