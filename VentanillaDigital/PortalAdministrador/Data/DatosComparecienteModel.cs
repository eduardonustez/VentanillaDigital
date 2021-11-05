namespace PortalAdministrador.Data
{
    public class DatosComparecienteModel
    {
        public long TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumentoNombre { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public bool HitDedo1 { get; set; }
        public bool HitDedo2 { get; set; }
    }
}
