using System.Collections.Generic;
using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Persona : EntidadBase
    {
        public long PersonaId { get; set; }
        public int TipoIdentificacionId { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string NumeroCelular { get; set; }
        public int? Genero { get; set; }
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public string tokenAuth { get; set; }
        public virtual IEnumerable<PersonaDatos> PersonaDatos { get; set; }
    }
}
