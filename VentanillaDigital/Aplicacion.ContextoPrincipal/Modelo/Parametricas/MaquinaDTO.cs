using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class MaquinaDTO : ResponseMaquinaDTO
    {
        public long MaquinaId { get; set; }
        public string Nombre { get; set; }
        public string MAC { get; set; }
        public string DireccionIP { get; set; }
        public int TipoMaquina { get; set; }
        public long NotariaId { get; set; }
        public bool MaquinaExiste { get; set; }
        public long IdCliente { get; set; }
        public string IdConvenio { get; set; }
        public long IdOficina { get; set; }
        public bool EsvalidoFinal { get; set; }
    }
}
