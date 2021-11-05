using System.Collections.Generic;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class PersonaDeleteRequestDTO : NewRegisterDTO
    {
        public List<string> Identificacion { get; set; }
    }
}
