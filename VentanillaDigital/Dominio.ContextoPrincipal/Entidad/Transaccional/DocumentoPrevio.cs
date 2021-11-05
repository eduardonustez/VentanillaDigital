using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class DocumentoPrevio : EntidadBase
    {
        public long DocumentoPrevioId { get; set; }
        public long TramiteId { get; set; }
        public byte[] Documento { get; set; }
        public string Seguimiento { get; set; }
        public int Tipo { get; set; }

        public virtual Tramite Tramite { get; set; }
    }
}
