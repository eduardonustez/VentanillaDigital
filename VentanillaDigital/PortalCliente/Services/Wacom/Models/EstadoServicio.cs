using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Wacom.Models
{
    public enum EstadoServicio
    {
        Desconectado,
        Verificando,
        ServicioConectado,
        ControladorConectado,
        DispositivoConectado
    }
}
