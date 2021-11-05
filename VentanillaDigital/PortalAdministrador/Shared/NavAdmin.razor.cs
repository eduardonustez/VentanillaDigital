using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Shared
{
    public partial class NavAdmin
    {
        [Parameter]
        public bool esRolUsuario { get; set; }


    }
}
