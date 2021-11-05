using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Text;
using TSAIntegrationLibrary.Entities;

namespace TSAIntegrationLibrary
{
    public interface ITSA
    {
        string AgregarEstampaCronologica(string pdfFile, TSAConfig tsaConfig, string signatureName);
    }
}
