using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ITemplateServicio : IDisposable
    {
        string ObtenerTemplateRecuperacionPassword(string email, string enlace);
        string ObtenerTemplateNotificacionTramiteCliente(string enlace, string nombreCliente);

        string ObtenerTemplateAsignacionClave(string email, string enlace);
    }
}
