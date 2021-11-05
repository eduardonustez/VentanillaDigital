using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class PersonaDatosDTO : NewRegisterDTO
    {
        public long PersonaDatosId { get; set; }
        public long PersonaId { get; set; }
        public int TipoDatoId { get; set; }
        public string Valor { get; set; }
    }
}
