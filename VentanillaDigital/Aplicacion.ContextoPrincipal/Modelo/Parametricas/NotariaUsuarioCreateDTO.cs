using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class NotariaUsuarioCreateDTO : NewRegisterDTO
    {
        public string UsuarioId { get; set; }
        public string UsuarioEmail { get; set; }
        public string Celular { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public bool SincronizoRNEC { get; set; }
    }
}
