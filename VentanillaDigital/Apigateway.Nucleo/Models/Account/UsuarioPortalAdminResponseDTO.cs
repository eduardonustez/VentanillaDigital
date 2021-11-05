using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class UsuarioCreacionPortalAdminResponseDTO
    {
        public bool Error { get; set; }
    }

    public class UsuarioPortalAdminResponseDTO
    {
        public long UsuarioAdministracionId { get; set; }
        public string Login { get; set; }
        public string Rol { get; set; }
    }
}
