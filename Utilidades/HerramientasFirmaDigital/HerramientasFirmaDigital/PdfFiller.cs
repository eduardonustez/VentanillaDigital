using HerramientasFirmaDigital.Abstraccion;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HerramientasFirmaDigital
{
    public class PdfFiller : IPdfFiller
    {
        public Dictionary<string, bool> CheckFields(byte[] pdfTemplate, string[] fieldsToCheck)
        {
            Dictionary<string, bool> resul = new Dictionary<string, bool>();
            List<string> fieldList = ListFieldNames(pdfTemplate);
            if (fieldsToCheck != null)
            {
                foreach (string fieldName in fieldsToCheck)
                {
                    resul.Add(fieldName, fieldList.Contains(fieldName));
                }
            }
            return resul;
        }

        public byte[] FillPdf(byte[] pdfTemplate, Dictionary<string, string> fields)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                foreach (var field in fields)
                    pdfFormFields.SetField(field.Key, field.Value);

                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                pdfReader.Close();
                return ms.ToArray();
            }

        }
        public byte[] StampPdf(byte[] pdfTemplate, byte[] imageBytes,
            float x, float y, float newHeight = 0, float newWidth = 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);

                if (newHeight > 0 && newWidth > 0)
                    image.ScaleAbsolute(newWidth, newHeight);

                image.SetAbsolutePosition(x, y);

                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    PdfContentByte content = pdfStamper.GetUnderContent(i);
                    content.AddImage(image);
                }

                pdfStamper.FormFlattening = false;
                pdfStamper.Close();
                pdfReader.Close();
                return ms.ToArray();
            }
        }

        public byte[] StampAllPages(byte[] pdfTemplate, byte[] firma, byte[] sello)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (MemoryStream sourceStream = new MemoryStream(pdfTemplate))
                {
                    // Modify PDF located at "source" and save to "target"
                    iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(
                        new iText.Kernel.Pdf.PdfReader(sourceStream),
                        new iText.Kernel.Pdf.PdfWriter(ms)
                    );
                    // Document to add layout elements: paragraphs, images etc
                    var document = new iText.Layout.Document(pdfDocument);
                    var imageDataFirma = iText.IO.Image.ImageDataFactory.Create(firma);
                    var imageDataSello = iText.IO.Image.ImageDataFactory.Create(sello);
                    // Create layout image object and provide parameters. Page number = 1

                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        iText.Kernel.Geom.Rectangle mediabox = pdfDocument.GetPage(i).GetPageSize();
                        var w1 = mediabox.GetWidth();
                        w1 = w1 - 70;
                        iText.Layout.Element.Image image1 = new iText.Layout.Element.Image(imageDataSello).ScaleAbsolute(50, 50).SetFixedPosition(i, w1, 70);
                        iText.Layout.Element.Image image2 = new iText.Layout.Element.Image(imageDataFirma).ScaleAbsolute(50, 50).SetFixedPosition(i, w1, 20);
                        // This adds the image to the page
                        document.Add(image1);
                        document.Add(image2);
                    }

                    pdfDocument.Close();

                    return ms.ToArray();
                }
            }
        }

        private List<string> ListFieldNames(byte[] file)
        {
            List<string> fieldList = new List<string>();
            PdfReader pdfReader = new PdfReader(file);
            foreach (DictionaryEntry de in pdfReader.AcroFields.Fields)
            {
                fieldList.Add(de.Key.ToString());
            }
            pdfReader.Close();
            return fieldList;
        }


    }
}
