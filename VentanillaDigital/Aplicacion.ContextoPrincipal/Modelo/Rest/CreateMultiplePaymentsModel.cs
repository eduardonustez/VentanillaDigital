namespace Aplicacion.ContextoPrincipal.Modelo.Rest
{
    public class CreateMultiplePaymentsModel
    {
        public long OrderAmount { get; set; }
        public long OrderTax { get; set; }
        public string Concept { get; set; }
        public string Email { get; set; }
        public string Cuandi { get; set; }
        public string NombreDestinatario { get; set; }
        public string DocumentBase64 { get; set; }
        public string Document2Base64 { get; set; }
        public string Document3Base64 { get; set; }
        public long TransaccionId { get; set; }
    }

    public class CreateMultiplePaymentsResponseModel
    {
        public string paymentLink { get; set; }
        public long itemReference { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
