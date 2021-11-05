using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Parametricas
{
    public class ValidarTramitePersonaDTO
    {
        public int NotariaId { get; set; }
        public string Email { get; set; }
        public int TipoIdentificacionId { get; set; }
        public string NumeroDocumento { get; set; }
    }

    public class ValidaTramiteResponseDTO
    {
        public bool TramiteExiste { get; set; }
        public DateTime? FechaCreacionTramite { get; set; }
    }
}
