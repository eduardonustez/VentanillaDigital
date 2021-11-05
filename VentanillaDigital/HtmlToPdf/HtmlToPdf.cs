using HiQPdf;
using System;

using System.IO;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.Collections;
using System.Drawing;
using HtmlToPdf.Entidades;

namespace HtmlToPdf
{
    public class HtmlToPdf : IHtmlToPdf
    {
        private const string HiqpdfLicense = "YSkIMDEF-By0IAxMA-ExhQU09R-QVBBUEFY-WFlWQVJQ-T1BTT1hY-WFg=";

        public string CreatePDF(PlantillaParametros plantilla)
        {
            var plantillaHTML = plantilla.Plantilla;
            plantillaHTML = GenerarHTML(plantilla, plantillaHTML);

            return RenderPdf(plantillaHTML, plantilla.footer,new ConfigDocumento());
        }
        public string CreateDocument(PlantillaParametros plantilla,ConfigDocumento configDocumento)
        {
            var plantillaHTML = plantilla.Plantilla;
            plantillaHTML = GenerarHTML(plantilla, plantillaHTML);

            return RenderPdf(plantillaHTML, plantilla.footer, configDocumento);
        }

        private string GenerarHTML(PlantillaParametros plantilla, string plantillaHTML)
        {
            foreach (var param in plantilla.Parametros)
            {
                if (plantillaHTML.Contains("[" + param.NombreCampo + "]"))
                {
                    plantillaHTML = plantillaHTML.Replace("[" + param.NombreCampo + "]", param.Valor);
                }
            }

            if (plantilla.Comparecientes != null && plantilla.Comparecientes.Count > 0)
            {
                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlNode nodo;
                html.LoadHtml(plantillaHTML);
                var bodyNode = html.DocumentNode.SelectSingleNode("//body");
                bodyNode.Attributes.Add("style", "background-color = 'transparent';");
                var tablaComparecientes = html.DocumentNode.SelectSingleNode("//*[contains(@id,'comparecientes')]");

                var comp1 = plantilla.Comparecientes[0];

                if (plantilla.Comparecientes.Count > 1)
                {
                    plantilla.Comparecientes.RemoveAt(0);
                    var auxComparecientes = DiligenciarPlantilla(tablaComparecientes.InnerHtml, plantilla.Comparecientes);
                    nodo = HtmlAgilityPack.HtmlNode.CreateNode("<div>" + auxComparecientes + "</div>");
                    tablaComparecientes.RemoveAllChildren();
                    tablaComparecientes.PrependChild(nodo);
                }
                else
                {
                    tablaComparecientes.RemoveAllChildren();
                }

                plantillaHTML = DiligenciarPlantillaParametro(html.DocumentNode.InnerHtml, comp1);
            }

            return plantillaHTML;
        }

        public string CreateSticker(PlantillaParametros plantilla)
        {
            var plantillaHTML = plantilla.Plantilla;
            string plantillaHTML2 = plantilla.Plantilla2;

            List<string> hojasDocumento = GenerarHTMLSticker(plantilla, plantillaHTML, plantillaHTML2);

            return RenderPdfSticker(hojasDocumento);
        }

        private List<string> GenerarHTMLSticker(PlantillaParametros plantilla, string plantillaHTML, string plantillaHTML2 = null)
        {
            List<string> hojasDocumento = new List<string>();

            if (plantilla.Comparecientes.Count == 2 && !string.IsNullOrEmpty(plantillaHTML2))
            {
                hojasDocumento.Add(AgregarHojaSticker(plantilla, plantillaHTML, 0));
                hojasDocumento.Add(AgregarHojaSticker(plantilla, plantillaHTML2, 1));
            }
            else
            {
                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(plantillaHTML);
                var bodyNode = html.DocumentNode.SelectSingleNode("//body");
                bodyNode.Attributes.Add("style", "background-color = 'transparent';");
                plantillaHTML = html.DocumentNode.InnerHtml;

                foreach (var param in plantilla.Parametros)
                {
                    if (plantillaHTML.Contains("[" + param.NombreCampo + "]"))
                    {
                        plantillaHTML = plantillaHTML.Replace("[" + param.NombreCampo + "]", param.Valor);
                    }
                }

                foreach (var compareciente in plantilla.Comparecientes)
                {
                    string copiaPlantillaHtml = plantillaHTML;
                    foreach (var param in compareciente)
                    {
                        if (copiaPlantillaHtml.Contains("[" + param.NombreCampo + "]"))
                        {
                            copiaPlantillaHtml = copiaPlantillaHtml.Replace("[" + param.NombreCampo + "]", param.Valor);
                        }
                    }
                    hojasDocumento.Add(copiaPlantillaHtml);
                }
            }
            return hojasDocumento;
        }

        private static string AgregarHojaSticker(PlantillaParametros plantilla, string plantillaHTML, int compareciente)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(plantillaHTML);
            var bodyNode = html.DocumentNode.SelectSingleNode("//body");
            bodyNode.Attributes.Add("style", "background-color = 'transparent';");
            plantillaHTML = html.DocumentNode.InnerHtml;

            foreach (var param in plantilla.Parametros)
            {
                if (plantillaHTML.Contains("[" + param.NombreCampo + "]"))
                {
                    plantillaHTML = plantillaHTML.Replace("[" + param.NombreCampo + "]", param.Valor);
                }
            }

            string copiaPlantillaHtml = plantillaHTML;
            foreach (var param in plantilla.Comparecientes[compareciente])
            {
                if (copiaPlantillaHtml.Contains("[" + param.NombreCampo + "]"))
                {
                    copiaPlantillaHtml = copiaPlantillaHtml.Replace("[" + param.NombreCampo + "]", param.Valor);
                }
            }

            return copiaPlantillaHtml;
        }

        public string DiligenciarPlantilla(string plantilla, List<IEnumerable<Parametro>> parametros)
        {
            string aux = "";

            foreach (var parametro in parametros)
            {
                aux += DiligenciarPlantillaParametro(plantilla, parametro);
            }
            return aux;
        }

        public string DiligenciarPlantillaParametro(string plantilla, IEnumerable<Parametro> parametro)
        {
            string aux = "";
            aux += plantilla;
            foreach (var param in parametro)
            {
                if (aux.Contains("[" + param.NombreCampo + "]"))
                {
                    aux = aux.Replace("[" + param.NombreCampo + "]", param.Valor);
                }
            }
            return aux;
        }

        public static string RenderPdf(string plantillaHtml, string[] footer,ConfigDocumento configDocumento)
        {
            HiQPdf.HtmlToPdf htmlToPdfConverter = new HiQPdf.HtmlToPdf
            {
                SerialNumber = HiqpdfLicense
            };

            htmlToPdfConverter.BrowserWidth = configDocumento.BrowserWidth;
            htmlToPdfConverter.Document.PageSize = PdfPageSize.Letter;
            htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.Document.FitPageHeight = false;
            htmlToPdfConverter.Document.FitPageWidth = false;
            htmlToPdfConverter.Document.ResizePageWidth = false;

            htmlToPdfConverter.Document.Compress = true;
            htmlToPdfConverter.Document.ImagesCompression = 50;

            //htmlToPdfConverter.Document.Margins = new PdfMargins(14);
            htmlToPdfConverter.Document.Margins = new PdfMargins(configDocumento.LeftMargin, 
                configDocumento.RightMargin,configDocumento.TopMargin, configDocumento.BottomMargin);
            if (footer.Length > 0)
            {
                htmlToPdfConverter.Document.Footer.Enabled = true;
                //PdfHtml footerHtml = new PdfHtml(5, 5, $@"<span style=""position: absolute;right: 30px;font-family: Calibri, Arial, sans-serif;font-size: 24px;"">{footer[0]}</span>", null);
                //footerHtml.FitDestHeight = true;
                //footerHtml.FontEmbedding = true;
                float footerHeight = htmlToPdfConverter.Document.Footer.Height;
                Font pageNumberingFont = new Font(new FontFamily("Calibri"), 12, GraphicsUnit.Point);
                PdfText TipoActaNumero = new PdfText(500, footerHeight - 20, $"{footer[0]}", pageNumberingFont);
                TipoActaNumero.HorizontalAlign = PdfTextHAlign.Default;
                TipoActaNumero.EmbedSystemFont = true;
                htmlToPdfConverter.Document.Footer.Layout(TipoActaNumero);
            }
            string pdfBase64 = "";
            if (configDocumento.UseBorder)
                pdfBase64 = Convert.ToBase64String(AgregarBorde(htmlToPdfConverter.ConvertHtmlToMemory(plantillaHtml, null), false));
            else
                pdfBase64 = Convert.ToBase64String(htmlToPdfConverter.ConvertHtmlToMemory(plantillaHtml, null));
            return pdfBase64;
        }

        public static string RenderPdfSticker(List<string> plantillaHtml)
        {
            HiQPdf.HtmlToPdf htmlToPdfConverter = new HiQPdf.HtmlToPdf
            {
                SerialNumber = HiqpdfLicense
            };

            HiQPdf.PdfDocument archivoConsolidado = new HiQPdf.PdfDocument()
            {
                SerialNumber = HiqpdfLicense
            };

            MemoryStream ms = new MemoryStream();

            htmlToPdfConverter.BrowserHeight = 374;
            htmlToPdfConverter.BrowserWidth = 302;
            htmlToPdfConverter.Document.PageSize = new PdfPageSize(226, 283);
            htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.Document.FitPageHeight = false;
            htmlToPdfConverter.Document.FitPageWidth = false;
            htmlToPdfConverter.Document.ResizePageWidth = false;

            htmlToPdfConverter.Document.Compress = true;
            htmlToPdfConverter.Document.ImagesCompression = 50;

            htmlToPdfConverter.Document.Margins = new PdfMargins(0);

            foreach (var item in plantillaHtml)
            {
                archivoConsolidado.AddDocument(htmlToPdfConverter.ConvertHtmlToPdfDocument(item, null));
            }

            archivoConsolidado.WriteToStream(ms);

            var pdfBase64 = Convert.ToBase64String(AgregarBorde(ms.ToArray(), true));

            return pdfBase64;
        }

        public static byte[] AgregarBorde(byte[] pdfBytes, bool isSticker)
        {
            Image borde = isSticker ? Properties.Resources.borde_sticker :
                                    Properties.Resources.borde_carta_full;

            try
            {
                using (Stream inputPdfStream = new MemoryStream(pdfBytes))
                using (MemoryStream outputPdfStream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(inputPdfStream);
                    PdfStamper stamper = new PdfStamper(reader, outputPdfStream);
                    Hashtable layers = stamper.GetPdfLayers();

                    foreach (KeyValuePair<string, PdfLayer> entry in layers)
                    {
                        PdfLayer layer = entry.Value;
                        layer.On = false;
                    }

                    iTextSharp.text.Image image = 
                        iTextSharp.text.Image.GetInstance(borde, (iTextSharp.text.BaseColor) null);
                    image.SetAbsolutePosition(0, 0);

                    for (int page = 1; page <= reader.NumberOfPages; ++page)
                    {
                        PdfContentByte canvas = stamper.GetUnderContent(page);

                        var pageSize = reader.GetPageSizeWithRotation(page);
                        image.ScaleAbsoluteHeight(pageSize.Height);
                        image.ScaleAbsoluteWidth(pageSize.Width);

                        canvas.SaveState();
                        canvas.AddImage(image);
                        canvas.RestoreState();
                    }

                    stamper.FormFlattening = true;
                    stamper.Close();
                    reader.Close();

                    return outputPdfStream.ToArray();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
