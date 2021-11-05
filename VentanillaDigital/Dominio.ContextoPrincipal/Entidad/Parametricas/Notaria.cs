using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Notaria:EntidadBase
    {
        public long NotariaId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public long? SelloId { get; set; }
        public SelloNotaria SelloArchivo { get; set; }
        public long UbicacionId { get; set; }
        public int NumeroNotaria { get; set; }
        public string NumeroNotariaEnLetras { get; set; }
        public string CirculoNotaria { get; set; }
        public long? NotarioEnTurno { get; set; }
        public virtual Ubicacion Ubicacion { get; set; }
        public virtual IEnumerable<NotariaUsuarios> NotariasUsuarios { get; set; }
        //public virtual ICollection<DocumentoPendienteAutorizar> DocumentosPendienteAutorizar { get; set; } = new HashSet<DocumentoPendienteAutorizar>();
    }
}
