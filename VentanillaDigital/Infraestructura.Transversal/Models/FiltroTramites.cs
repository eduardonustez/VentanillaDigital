using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Models
{
    public class FiltroTramites
    {
        public long NotariaId { get; set; }
        public long TramiteId { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public String NuipOperador { get; set; }
        public String NuipCompareciente { get; set; }
        public int NumeroPagina { get; set; }
        public int RegistrosPagina { get; set; }
    }
}
