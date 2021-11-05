using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteComparecienteReturnDTO
    {
        public int TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public long TransaccionRNECId { get; set; }
        public List<ArchivosComparecientesDTO> Archivos { get; set; }
    }
}
