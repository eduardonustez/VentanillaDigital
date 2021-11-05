namespace ApiGateway.Contratos.Models
{
    public class TramiteInfoBasica
    {
        public long TramiteId { get; set; }
        public int CantidadComparecientes { get; set; }
        public long TipoTramiteId { get; set; }
        public string TipoTramite { get; set; }
        public long NotariaId { get; set; }
        public string NotariaNombre { get; set; }
    }
}
