using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Models
{
    public class EnviarRecaudoModel
    {
        public List<long> ArchivosPortalVirtualId { get; set; }
        public List<ArchivoRecaudoModel> Archivos { get; set; }
    }

    public class ArchivoRecaudoModel
    {
        public int Index { get; set; }
        public string Nombre { get; set; }
        public string Formato { get; set; }
        public string Data { get; set; }
        public int? Type { get; set; }
    }
}
