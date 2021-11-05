using Infraestructura.Transversal.Models;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class ConfiguracionesNotariaRequestDTO: DefinicionFiltroSimple
    {
        public string CorreoUsuario { get; set; }
        public long NotariaId { get; set; }
    }
}
