using System;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class HistorialTramite
    {
        public DateTime Fecha { get; set; }
        public string Detalle { get; set; }
        public string Documento { get; set; }
        public string Persona { get; set; }
    }
}
