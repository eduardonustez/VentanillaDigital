namespace Infraestructura.KeyManager.Models
{
    public class ResendEmailRequest
    {
        public int CertificateId { get; set; }
        public string UserId { get; set; }
    }
}