using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class NotariaUsuarios:EntidadBase
    {
        public long NotariaUsuariosId { get; set; }
        public long  NotariaId { get; set; }
        public string UsuariosId { get; set; }
        public string UserEmail { get; set; }
        public long PersonaId { get; set; }
        public string Celular { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public bool SincronizoRNEC { get; set; }
        //public TipoSexo? Sexo { get; set; }
        public virtual Notaria Notaria { get; set; }
        public virtual Notario Notario { get; set; }
        public virtual Persona Persona { get; set; }
        //public virtual ICollection<DocumentoPendienteAutorizar> DocumentosPendienteAutorizar { get; set; } = new HashSet<DocumentoPendienteAutorizar>();
    }

    public enum TipoSexo
    {
        Hombre,
        Mujer
    }
}
