using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Correo
{
    public interface IManejadorCorreos
    {
        Task<RespuestaEnvioCorreo> EnviarCorreo(ICollection<string> destinatarios, 
             string asunto, string mensajeHtml, AlternateView htmlView, 
             IEnumerable<System.Net.Mail.Attachment> adjuntos = null);

    }
}
