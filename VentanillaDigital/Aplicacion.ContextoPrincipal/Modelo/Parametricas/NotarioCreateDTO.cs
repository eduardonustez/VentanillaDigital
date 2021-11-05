using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class NotarioCreateDTO : ModeloDTO
    {
        public string Email { get; set; }
        public string Grafo { get; set; }
        public string Pin { get; set; }

    }
    public class ValSolicitudPinDTO : NewRegisterDTO
    {
        public string Clave { get; set; }
    }

}
