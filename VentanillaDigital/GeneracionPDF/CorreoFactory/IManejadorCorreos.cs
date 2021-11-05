#region Directivas
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Generacion_PDF_Notaria;
using Generacion_PDF_Notaria.Models;
#endregion

namespace CorreoFactory
{
    /// <summary>
    /// Define el contrato para el envío de correos.
    /// </summary>
    public interface IManejadorCorreos
    {

        #region Métodos

        /// <summary>
        /// Enviar correo electrónico
        /// </summary>
        /// <param name="apiKey">SendGrid ApiKey</param>
        /// <param name="correoEnvia">Correo electrónico de envio</param>
        /// <param name="nombreCorreoEnvia">Nombre del usuario asociado al correo electrónico de envio</param>
        /// <param name="asunto">Asunto</param>
        /// <param name="destinatarios">Correos electrónicos a donde se enviará el correo</param>
        /// <param name="mensajeHtml">Html del mensaje</param>
        /// <param name="mensajeTexto">TExto del mensaje</param>
        /// <param name="adjuntos">Adjuntos que se enviaran en el correo, puede ser null</param>
        /// <param name="usarBCC">En el caso en que sean varios destinatarios indica si se debe enviar a un destinatario y el resto por bcc</param>
        /// <returns></returns>
        bool EnviarCorreo(ServidorCorreo objServidor, ICollection<string> destinatarios, string nombreDestinatario, string asunto, string mensajeHtml, bool usarBCC, IEnumerable<Attachment> adjuntos);
               
        #endregion

    }
}
