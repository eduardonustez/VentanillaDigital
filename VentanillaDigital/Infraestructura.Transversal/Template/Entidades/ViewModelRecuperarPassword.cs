using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Template.Entidades
{
    public class ViewModelRecuperarPassword :  ViewModelBase
    {
        public string NombreNotario { get; set; }
        public string DescripcionNotaria { get; set; }
        public string CallbackUrl { get; set; }
    }
}
