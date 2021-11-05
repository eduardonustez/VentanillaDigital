using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generacion_PDF_Notaria.Models
{
    public class RespuestaBase
    {
        /// <summary>
        /// Indica si la operación se realizó exitosamente
        /// </summary>

        public bool OperacionExitosa { get; set; }

        /// <summary>
        /// Mensaje asociado a la respuesta
        /// </summary>
        public string Mensaje { get; set; }
    }
}
