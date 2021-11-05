namespace Infraestructura.Transversal.Models
{
    public class ActualizarPagoModel
    {
        public decimal ValorPagado { get; set; }
        public string Referencia { get; set; }
        public int Estado { get; set; } // 1 - Generado, 2 - Enviado, 3 - Pagado, 4 - Anulado, 5 - Rechazado 
    }
}
