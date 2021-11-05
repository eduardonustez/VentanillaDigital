namespace Infraestructura.KeyManager.Models
{
    public class PinChangeRequest
    {
        public int RequestId { get; set; }
        public string PinOld { get; set; }
        public string PinNew { get; set; }
    }
}