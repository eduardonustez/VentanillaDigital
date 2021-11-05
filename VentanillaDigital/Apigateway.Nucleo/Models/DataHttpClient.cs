using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models
{
    public class DataHttpClient
    {
        public string ServiceBaseAddress { get; set; }
        public string TipoTokenBasic { get; set; }
        public string TokenBasic { get; set; }
        public string NombreTokenBasic { get; set; }
        public string TipoTokenBearer { get; set; }
        public string TokenBearer { get; set; }
        public string NombreTokenBearer { get; set; }
    }
}
