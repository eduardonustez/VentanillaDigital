namespace ApiGateway.Contratos.Models.Notario
{
    public class ActoNotarialModel
    {
        public int ActoNotarialId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int? TipoTramiteVirtualId { get; set; }
    }
}
