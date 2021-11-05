using System.Collections.Generic;

namespace HtmlToPdf.Entidades
{
    public class PlantillaParametros
    {
        public string Plantilla { get; set; }
        public string Plantilla2 { get; set; }
        public string[] footer { get; set; }
        public IEnumerable<Parametro> Parametros { get; set; }
        public List<IEnumerable<Parametro>> Comparecientes { get; set; }
    }
}
