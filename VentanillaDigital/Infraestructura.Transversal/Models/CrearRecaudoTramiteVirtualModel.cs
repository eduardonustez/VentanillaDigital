namespace Infraestructura.Transversal.Models
{
    public class CrearRecaudoTramiteVirtualModel
    {
        public int TramitePortalVirtualId { get; set; }
        public string NombreCliente { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Correo { get; set; }
        public decimal Valor { get; set; }
        public decimal IVA { get; set; }
        public string Observacion { get; set; }
    }

    public class ResponseCrearRecaudo
    {
        public bool Status { get; set; }
    }
}
