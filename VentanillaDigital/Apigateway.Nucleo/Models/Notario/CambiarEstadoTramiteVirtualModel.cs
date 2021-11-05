using Infraestructura.Transversal.Models;
using System.Collections.Generic;

namespace ApiGateway.Contratos.Models.Notario
{
    public class CambiarEstadoTramiteVirtualModel
    {
        public int Estado { get; set; }
        public decimal Precio { get; set; }
        public string Razon { get; set; }
        public string ClaveTestamento { get; set; }
        public int? ActoPrincipalId { get; set; }
        public List<UploadFileModel> Files { get; set; }
        public Coordenadas Coordenadas { get; set; }
        public List<ActoPorTramiteModel> ActosNotariales { get; set; }
        public string DatosAdicionalesCierre { get; set; }
    }

    public class CambiarEstadoTramiteVirtualClienteModel
    {
        public string Usuario { get; set; }
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public List<UploadFileModel> Files { get; set; }
    }

    public class Coordenadas
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
