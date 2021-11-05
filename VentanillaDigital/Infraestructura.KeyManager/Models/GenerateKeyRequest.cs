namespace Infraestructura.KeyManager.Models
{
    public class GenerateKeyRequest
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string FirstName	{get;set;}
        public string Department{get;set;}
        public string Notary{get;set;}
        public string Dni{get;set;}
        public string DniPublicNotary{get;set;}
        public string City{get;set;}
        public string NotaryAddress{get;set;}
        public string SurName{get;set;}
        public string Position{get;set;}
        public string PhoneNumber { get; set; }
        public string Pin { get; set; }
    }
}