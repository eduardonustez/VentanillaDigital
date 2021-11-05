using System;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteDetalleDTO
    {
        public long TramiteId { get; set; }
        public bool ComparecientesCompletos { get; set; }
        public long ComparecienteActual { get; set; }
        public int CantidadComparecientes { get; set; }
        
        public DateTime Fecha { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Estado { get; set; }
        public string TipoTramite { get; set; }
        public string NombreNotaria { get; set; }
        public bool ActaGenerada { get; set; }
    }
}
