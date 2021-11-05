using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class MaquinaReturn
    {
        public long MaquinaId { get; set; }
        public string Nombre { get; set; }
        public string MAC { get; set; }
        public string DireccionIP { get; set; }
        public int TipoMaquina { get; set; }
        public long NotariaId { get; set; }
        public bool MaquinaExiste { get; set; }
    }
}
