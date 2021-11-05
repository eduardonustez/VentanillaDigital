using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class DocumentoPendienteAutorizar : EntidadBase
    {
        public long DocumentoPendienteAutorizarId { get; set; }
        public long TramiteId { get; set; }
        public long NotariaId { get; set; }
        public long NotarioUsuarioId { get; set; }
        public bool Generado { get; set; }
        public EstadoDocumento Estado { get; set; }
        public short Intentos { get; set; }
        public string Error { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public string Seguimiento { get; set; }
        public bool UsarSticker { get; set; }
        public string MachineName { get; set; }
        public virtual Tramite Tramite { get; set; }
        public virtual Notaria Notaria { get; set; }
    }

    public enum EstadoDocumento : short
    {
        PENDIENTE = 1,
        EN_PROCESO,
        GENERADO,
        ERROR
    }
}
