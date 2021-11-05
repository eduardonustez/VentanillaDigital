using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdf.Entidades
{
    public class Parametro
    {
        public string NombreCampo { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }

        public Parametro()
        {

        }
        public Parametro(string nombreCampo, string valor, string tipo)
        {
            NombreCampo = nombreCampo;
            Valor = valor;
            Tipo = tipo;
        }
    }
}
