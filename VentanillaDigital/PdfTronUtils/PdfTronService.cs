using pdftron.PDF;
using System.Collections.Generic;
using System.IO;

namespace PdfTronUtils
{
    public interface IPdfTronService
    {
        byte[] FillPdf(byte[] pdfTemplate, Dictionary<string, string> fields);
    }

    public class PdfTronService : IPdfTronService
    {
        public byte[] FillPdf(byte[] pdfTemplate, Dictionary<string, string> fields)
        {
            using var ms = new MemoryStream(pdfTemplate);
            PDFDoc doc = new PDFDoc(ms);
            ContentReplacer replacer = new ContentReplacer();

            for (int i = 1; i <= doc.GetPageCount(); i++)
            {
                Page page = doc.GetPage(i);

                foreach (var item in fields)
                {
                    replacer.AddString(item.Key, item.Value);
                }

                replacer.Process(page);
            }

            var document = doc.Save(pdftron.SDF.SDFDoc.SaveOptions.e_compatibility);
            doc.Close();

            return document;
        }
    }
}
