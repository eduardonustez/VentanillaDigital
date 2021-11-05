namespace ApiGateway.Contratos.Models.Account
{
    public class UsuarioModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RolId { get; set; }
        public string RolName { get; set; }
        public long NotariaId { get; set; }
        public string NotariaName { get; set; }
        public string Documento { get; set; }
        public long PersonaId { get; set; }
        public string PersonaName { get; set; }
    }
}
