using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models.Transaccional
{
    public class ComparecienteCreateModel
    {
        public int TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public long TransaccionRNECId { get; set; }
        public List<Archivo> Archivo { get; set; }
    }

    public class Archivo
    {
        public int TipoArchivo { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamanio { get; set; }
        public string Ruta { get; set; }
        public string Contenido { get; set; }
    }
}
