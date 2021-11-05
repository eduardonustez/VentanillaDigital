using HtmlToPdf.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdf
{
    public interface IHtmlToPdf
    {
        string CreatePDF(PlantillaParametros plantilla);
        string CreateDocument(PlantillaParametros plantilla, ConfigDocumento configDocumento);
        string CreateSticker(PlantillaParametros plantilla);
    }
}
