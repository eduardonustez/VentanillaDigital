using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models.Transaccional
{
    public class TramiteComparecienteResponse
    {
        public PersonaInfoReturnDTO DatosBiograficos { get; set; }
        public string Foto { get; set; }
        public string Documeto { get; set; }
        public string Firma { get; set; }
        public string Huella { get; set; }
        public long TransaccionRNECId { get; set; }
        public DateTime FechaValidacion { get; set; }
        public Enum TipoArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public long Tamaño { get; set; }
    }
}
