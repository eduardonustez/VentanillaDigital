using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    public class ValidarIdentidadMovilRequest
    {
        public string Login { get; set; }
        public string IdOficina { get; set; }
        public long IdProducto { get; set; }
        public string NumeroDocumento { get; set; }
        public long IdCliente { get; set; }
        public string Convenio { get; set; }

    }
}
