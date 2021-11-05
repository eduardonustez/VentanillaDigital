using System;

namespace ApiGateway.Contratos
{
    public class ObtenerActaNotarialSeguraRequest
    {
        public int NotariaId { get; set; }
        public DateTime FechaTramite { get; set; }
        public string Email { get; set; }
        public string TramiteId { get; set; }
    }
}