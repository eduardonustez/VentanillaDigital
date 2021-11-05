using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasFirmaDigital.Abstraccion
{
    public interface IPdfFiller
    {
        byte[] FillPdf(byte[] pdfTemplate, Dictionary<string, string> fields);
        Dictionary<string, bool> CheckFields(byte[] pdfTemplate, string[] fieldsToCheck);
        byte[] StampPdf(byte[] pdfTemplate, byte[] imageBytes, 
            float x, float y, float newHeight = 0, float newWidth = 0);
        byte[] StampAllPages(byte[] pdfTemplate, byte[] firma, byte[] sello);
    }
}
