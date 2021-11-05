using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HerramientasFirmaDigital.Test
{
    public class AdjuntadorPdfItextTest
    {
        [Fact]
        public void PruebaAdjuntador()
        {
            var repeticiones = 5;
            var archivo = Properties.Resources.TestPdf;
            int paginasArchivo = ContarPaginas(archivo);
            int paginasEsperadas = repeticiones * paginasArchivo;

            var sut = new AdjuntadorPdfItext(archivo);
            for(int i=0;i<repeticiones-1;i++)
            {
                sut.Adjuntar(archivo);
            }

            var final = sut.Generar();
            int paginasFinal = ContarPaginas(final);
            Assert.Equal(paginasEsperadas, paginasFinal);
            //System.IO.File.WriteAllBytes("ou.pdf", final);
        }

        private int ContarPaginas (byte[] archivo)
        {
            var reader = new PdfReader(archivo);
            var paginasArchivo = reader.NumberOfPages;
            reader.Close();
            return paginasArchivo;
        }
    }
}
