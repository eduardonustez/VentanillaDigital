namespace ApiGateway.Contratos.Models.Reportes
{
    public class ReporteRequest
    {
        public string TipoReporte { get; set; }
    }

    public enum TipoReporte
    {
        ReporteOperacionalDiario = 1
    }
}
