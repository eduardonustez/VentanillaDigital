using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.NotariaVirtual
{
    public class ObtenerMiConfiguracionMiFirmaResponse
    {
        public string Gateway { get; set; }
        public string MyFrame { get; set; }
        public string ChannelAuthMiFirma { get; set; }
        public string ConfigurationGuid { get; set; }
        public string LoginConvenio { get; set; }
        public string PasswordConvenio { get; set; }
    }
}
