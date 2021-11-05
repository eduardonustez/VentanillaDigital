using GeneradorQR;
using HerramientasFirmaDigital.Interno.Abstraccion;
using HtmlToPdf;
using HtmlToPdf.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasFirmaDigital.Interno
{
    internal class GeneradorPaginaFirma : IGeneradorPaginaFirma
    {
        private readonly IGeneradorQR _generadorQR;
        private readonly IHtmlToPdf _htmlToPdf;

        public GeneradorPaginaFirma(IGeneradorQR generadorQR,
            IHtmlToPdf htmlToPdf)
        {
            _generadorQR = generadorQR;
            _htmlToPdf = htmlToPdf;
        }

        public byte[] GenerarPagina(DatosFirma datosFirma)
        {
            var parametros = ObtenerParametros(datosFirma);
            var nuevaPagina = _htmlToPdf.CreatePDF(parametros);
            return Convert.FromBase64String(nuevaPagina);
        }

        private PlantillaParametros ObtenerParametros(DatosFirma datosFirma)
        {
            string grafoDataUrl =
                $"data:image/{datosFirma.FormatoGrafo};" +
                $"base64,{Convert.ToBase64String(datosFirma.Grafo)}";

            string selloDataUrl =
                $"data:image/{datosFirma.FormatoSello};" +
                $"base64,{Convert.ToBase64String(datosFirma.Sello)}";

            PlantillaParametros plantillaParametros = new PlantillaParametros();
            var parametros = new List<Parametro>()
            {
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.Titulo),
                    Valor = datosFirma.Titulo
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.NombreCompleto),
                    Valor = datosFirma.NombreCompleto
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.Cargo),
                    Valor = datosFirma.Cargo
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.Grafo),
                    Valor = grafoDataUrl
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.Sello),
                    Valor = selloDataUrl
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.TextoFirma),
                    Valor = datosFirma.TextoFirma
                },
                new Parametro()
                {
                    NombreCampo = nameof(datosFirma.UrlQR),
                    Valor = _generadorQR.StringToQR(datosFirma.UrlQR)
                },
            };

            plantillaParametros.Parametros = parametros;
            plantillaParametros.Plantilla = Properties.Resources.PlantillaPaginaAdicional;
            plantillaParametros.Plantilla2 = null;
            plantillaParametros.footer = new string[0];

            return plantillaParametros;
        }
    }
}
