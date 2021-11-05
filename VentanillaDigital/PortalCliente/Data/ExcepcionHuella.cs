using PortalCliente.Services.Biometria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Data
{
    public class ExcepcionHuella
    {
        public string Descripcion { get; set; }
        public Dedo[] Dedos { get; set; }
    }
}
