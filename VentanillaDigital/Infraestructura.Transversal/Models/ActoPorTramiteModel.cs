using System;

namespace Infraestructura.Transversal.Models
{
    public class ActoPorTramiteModel
    {
        public long ActoPorTramiteId { get; set; }
        public int ActoNotarialId { get; set; }
        public string ActoNotarialNombre { get; set; }
        public string Cuandi { get; set; }
        public short Orden { get; set; }
        public DateTime Fecha { get; set; }
    }
}
