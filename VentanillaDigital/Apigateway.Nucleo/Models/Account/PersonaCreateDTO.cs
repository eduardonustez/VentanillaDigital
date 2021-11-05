namespace ApiGateway.Models
{
    public class PersonaCreateDTO 
    {
        public int TipoIdentificacionId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        //public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
    }
}
