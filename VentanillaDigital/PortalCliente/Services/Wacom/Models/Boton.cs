using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Wacom.Models
{
    public class Boton
    {
        public Rectangle Posicion { get; set; }
        public Func<Task> Accion { get; set; }
    }
}
