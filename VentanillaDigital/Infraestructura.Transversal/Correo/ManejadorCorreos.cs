using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Correo
{
    public class ManejadorCorreos : IManejadorCorreos
    {
        public ServidorCorreo _servidorCorreo { get; set; }
        public ManejadorCorreos(ServidorCorreo servidorCorreo)
        {
            _servidorCorreo = servidorCorreo;
        }
        public async Task<RespuestaEnvioCorreo> EnviarCorreo(ICollection<string> destinatarios, 
            string asunto, string mensajeHtml, AlternateView htmlView, 
            IEnumerable<Attachment> adjuntos = null)
        {
            RespuestaEnvioCorreo respuesta = new RespuestaEnvioCorreo();
            respuesta.ResultadoOk = false;
            try
            {
                MailAddress objCorreoDe = new MailAddress(_servidorCorreo.fromaddress, _servidorCorreo.fromname);
                //MailAddress objCorreoPara = new MailAddress(destinatarios.ToList()[0], nombreDestinatario);
                MailAddress objCorreoPara = new MailAddress(destinatarios.ToList()[0]);
                MailMessage objMailMessage = new MailMessage(objCorreoDe, objCorreoPara);
                objMailMessage.Subject = asunto;
                objMailMessage.Body = mensajeHtml;

                objMailMessage.IsBodyHtml = true;
                if (htmlView != null)
                    objMailMessage.AlternateViews.Add(htmlView);

                if (string.IsNullOrEmpty(_servidorCorreo.fromaddress))
                    throw new ArgumentNullException(nameof(_servidorCorreo.fromaddress));

                if (string.IsNullOrEmpty(_servidorCorreo.fromname))
                    throw new ArgumentNullException(nameof(_servidorCorreo.fromname));

                if (string.IsNullOrEmpty(asunto))
                    throw new ArgumentNullException(nameof(asunto));

                if (string.IsNullOrEmpty(mensajeHtml))
                    throw new ArgumentNullException(nameof(mensajeHtml));

                if (destinatarios.Count() == 0)
                    throw new ArgumentNullException(nameof(destinatarios));

                //if (usarBCC)
                //{
                //    destinatarios.ToList().Skip(1).ToList().ForEach(d =>
                //    {
                //        objMailMessage.Bcc.Add(d);
                //    });
                //}
                //else
                //{
                destinatarios.ToList().Skip(1).ToList().ForEach(d =>
                {
                    objMailMessage.To.Add(d);
                });
                //}

                if (adjuntos != null)
                {
                    foreach (var s in adjuntos)
                    {
                        objMailMessage.Attachments.Add(s);
                    }
                }

                SmtpClient servidor = new SmtpClient();
                servidor.Host = _servidorCorreo.host;

                if (!string.IsNullOrEmpty(_servidorCorreo.port))
                    servidor.Port = Convert.ToInt32(_servidorCorreo.port);

                if (!string.IsNullOrEmpty(_servidorCorreo.password))
                {
                    var basicCredential = new NetworkCredential(_servidorCorreo.username, _servidorCorreo.password);
                    servidor.UseDefaultCredentials = true;
                    servidor.Credentials = basicCredential;
                    servidor.EnableSsl = true;
                }

                servidor.SendCompleted += (s, e) =>
                {
                    servidor.Dispose();
                    objMailMessage.Dispose();
                };

                await servidor.SendMailAsync(objMailMessage);

                respuesta.ResultadoOk = true;
                return respuesta;
            }
            catch (ArgumentNullException)
            {
                respuesta.ResultadoOk = false;
                return respuesta;
            }
            catch (Exception exception)
            {
                respuesta.ResultadoOk = false;
                respuesta.Mensaje = exception.Message;
                return respuesta;
            }
        }
    }
}
