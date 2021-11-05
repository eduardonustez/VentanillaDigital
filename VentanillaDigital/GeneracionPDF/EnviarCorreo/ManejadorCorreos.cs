#region Directivas
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using CorreoFactory;
using Generacion_PDF_Notaria.Models;
#endregion

namespace Generacion_PDF_Notaria.EnviarCorreo
{
    /// <summary>
    /// Implementación manejador de correos con SendGrid
    /// </summary>

    public sealed class ManejadorCorreos : IManejadorCorreos
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Miembros IManejadorCorreos

        /// <summary>
        /// Envia correo electrónico según los parámetros especificados
        /// </summary>
        /// <param name="correoEnvia">Correo electrónico de envio</param>
        /// <param name="nombreCorreoEnvia">Nombre del usuario asociado al correo electrónico de envio</param>
        /// <param name="asunto">Asunto</param>
        /// <param name="destinatarios">Correos electrónicos a donde se enviará el correo</param>
        /// <param name="mensajeHtml">Html del mensaje con el texto que se enviará</param>
        /// <param name="Attachment">Archivos convertidos a bytes con su respectivo nombre</param>
        /// <param name="usarBCC">En el caso en que sean varios destinatarios indica si se debe enviar a un destinatario y el resto por BCC (Blind Carbon Copy)</param>
        /// <returns></returns>
        public bool EnviarCorreo(ServidorCorreo objServidor, ICollection<string> destinatarios, string nombreDestinatario, string asunto, string mensajeHtml, bool usarBCC, IEnumerable<Attachment> adjuntos)
        {
            // log.Info($"Ingreso Correo objServidor: {objServidor} nombreDestinatario {nombreDestinatario} asunto {asunto}");
            bool respuesta = false;
            try
            {
                MailAddress objCorreoDe = new MailAddress(objServidor.usuarioFrom, objServidor.nombreFrom);
                MailAddress objCorreoPara = new MailAddress(destinatarios.ToList()[0], nombreDestinatario);
                MailMessage objMailMessage = new MailMessage(objCorreoDe, objCorreoPara);
                objMailMessage.Subject = asunto;
                objMailMessage.Body = mensajeHtml;
                objMailMessage.IsBodyHtml = true;

                if (string.IsNullOrEmpty(objServidor.usuarioFrom))
                    throw new ArgumentNullException(nameof(objServidor.usuarioFrom));

                if (string.IsNullOrEmpty(objServidor.nombreFrom))
                    throw new ArgumentNullException(nameof(objServidor.nombreFrom));

                if (string.IsNullOrEmpty(asunto))
                    throw new ArgumentNullException(nameof(asunto));

                if (string.IsNullOrEmpty(mensajeHtml))
                    throw new ArgumentNullException(nameof(mensajeHtml));

                if (destinatarios.Count() == 0)
                    throw new ArgumentNullException(nameof(destinatarios));

                if (usarBCC)
                {
                    destinatarios.ToList().Skip(1).ToList().ForEach(d =>
                    {
                        objMailMessage.Bcc.Add(d);
                    });
                }
                else
                {
                    destinatarios.ToList().Skip(1).ToList().ForEach(d =>
                    {
                        objMailMessage.To.Add(d);
                    });
                }

                if (adjuntos != null)
                {
                    foreach (var s in adjuntos)
                    {
                        objMailMessage.Attachments.Add(s);
                    }
                }

                SmtpClient servidor = new SmtpClient();
                servidor.Host = objServidor.host;
                // log.Info($"Realiza la conexion a SMTP");
                if (!string.IsNullOrEmpty(objServidor.port))
                    servidor.Port = Convert.ToInt32(objServidor.port);

                if (!string.IsNullOrEmpty(objServidor.password))
                {
                    var basicCredential = new NetworkCredential(objServidor.usuarioFrom, objServidor.password);
                    servidor.UseDefaultCredentials = false;
                    servidor.Credentials = basicCredential;
                }
                // log.Info($"Envia correo");
                servidor.SendCompleted += (s, e) =>
                {
                    servidor.Dispose();
                    objMailMessage.Dispose();
                };

                servidor.Send(objMailMessage);
                respuesta = true;
                // log.Error($"Repuesta Correo Metodo: {respuesta.OperacionExitosa}");
                return respuesta;


            }
            catch (ArgumentNullException)
            {
                respuesta = false;
                // log.Error($"Error enviar correo : {exception}");
                return respuesta;
            }
            catch (Exception exception)
            {
                respuesta = false;
                // log.Error($"Error enviar correo : {exception}");
                return respuesta;

            }

        }
        #endregion



    }
}
