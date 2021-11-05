using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Notario:EntidadBase
    {
        public long NotarioId { get; set; }
        public string Nit { get; set; }
        public long? GrafoId { get; set; }
        public GrafoNotario GrafoArchivo { get; set; }
        public string Pin { get; set; }
        public int TipoNotario { get; set; }
        public long NotariaUsuariosId { get; set; }
        public string UsuarioCertificado { get; set; }
        public int? Certificadoid { get; set; }
        //public bool UsarSticker { get; set; }
        public virtual NotariaUsuarios NotariaUsuarios { get; set; }

    }
}
