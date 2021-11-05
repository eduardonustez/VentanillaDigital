using PortalAdministrador.Services.Biometria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data
{
    public class ExcepcionHuella
    {
        public string Descripcion { get; set; }
        public Dedo[] Dedos { get; set; }
    }
}
