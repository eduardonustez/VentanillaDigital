using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generacion_PDF_Notaria.Models
{
    public class ServidorCorreo
    {
        /// <summary>
        /// Servidor SMPT
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// Puerto
        /// </summary>
        public string port { get; set; }

        /// <summary>
        /// Contraseña
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// usuario de correo y remitente del correo
        /// </summary>
        public string usuarioFrom { get; set; }

        /// <summary>
        /// nombre remitente del correo
        /// </summary>
        public string nombreFrom { get; set; }
    }
}
