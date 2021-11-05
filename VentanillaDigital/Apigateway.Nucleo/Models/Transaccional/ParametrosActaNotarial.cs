using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class ParametrosActaNotarial
    {
        public string Plantilla { get; set; }
        public string Plantilla2 { get; set; }
        public string[] footer { get; set; }
        public IEnumerable<Parametro> Parametros { get; set; }
        public List<IEnumerable<Parametro>> Comparecientes { get; set; }
    }

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
