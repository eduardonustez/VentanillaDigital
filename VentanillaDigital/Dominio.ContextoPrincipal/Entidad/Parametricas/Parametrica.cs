using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Parametrica : EntidadBase
    {
        public long ParametricaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public int Longitud { get; set; }
        public string Expresion { get; set; }
    }
}
