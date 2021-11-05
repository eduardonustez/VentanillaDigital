using System.Collections.Generic;

namespace ApiGateway.Contratos.Models.Account
{
    public class MiFirmaResponse
    {
        public string MyFrame { get; set; }
        public string LoginConvenio { get; set; }
        public string Titulo { get; set; }
        public string Token { get; set; }
        public string CUANDI { get; set; }
        public string ConfigurationGuid { get; set; }
    }
}
