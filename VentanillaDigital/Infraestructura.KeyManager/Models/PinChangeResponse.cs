namespace Infraestructura.KeyManager.Models
{
    public class PinChangeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public string TimeStamp { get; set; }
    }
}