using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models
{
    public class NotarioDTOResponse
    {
        public long NotarioId { get; set; }
        public string Nit { get; set; }
        public string Grafo { get; set; }
        public string Pin { get; set; }
        public int TipoNotario { get; set; }
        public long NotariaUsuariosId { get; set; }
    }
    public class NotarioReturnModel
    {
        public long NotarioId { get; set; }
        public string NotarioNombre { get; set; }
    }

}
