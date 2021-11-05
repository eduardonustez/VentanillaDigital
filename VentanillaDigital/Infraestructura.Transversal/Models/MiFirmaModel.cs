using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Models
{
    public class MiFirmaModel
    {
        public string MyFrame { get; set; }
        public string LoginConvenio { get; set; }
        public string Titulo { get; set; }
        public string Token { get; set; }
        public string ConfigurationGuid { get; set; }
        public string CUANDI { get; set; }
        public List<ComparecientesIFrame> Comparecientes { get; set; }
    }

    public class ComparecientesIFrame
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}
