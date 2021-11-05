using HerramientasFirmaDigital.Abstraccion;
using HerramientasFirmaDigital.Interno;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HerramientasFirmaDigital
{
    public class AdjuntadorPdfItextFactory : IAdjuntadorPdfFactory
    {
        public IAdjuntadorPdf Obtener(byte[] documento)
            => new AdjuntadorPdfItext(documento);
    }

    public class AdjuntadorPdfItext : IAdjuntadorPdf
    {
        private Document _document;
        private MemoryStream _memoryStream;
        private PdfWriter _pdfWriter;
        private PdfContentByte _pageContentByte;
        public AdjuntadorPdfItext(byte[] documentoOriginal)
        {
            _document = new Document();
            _memoryStream = new MemoryStream();
            _pdfWriter = PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();

            _pageContentByte = _pdfWriter.DirectContent;

            Adjuntar(documentoOriginal);
        }

        public IAdjuntadorPdf Adjuntar(byte[] documento)
        {
            var reader = new PdfReader(documento);
            for (int currentPage = 1; currentPage <= reader.NumberOfPages; currentPage++)
            {
                _document.SetPageSize(reader.GetPageSize(currentPage));
                _document.NewPage();
                var page = _pdfWriter.GetImportedPage(reader, currentPage);
                _pageContentByte.AddTemplate(page, 0, 0);
            }
            return this;
        }

        public byte[] Generar()
        {
            _memoryStream.Flush();
            _document.Close();
            var ret = _memoryStream.ToArray();
            _memoryStream.Close();
            return ret;
        }
    }
}
