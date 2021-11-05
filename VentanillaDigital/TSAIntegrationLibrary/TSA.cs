using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Signatures;
using TSAIntegrationLibrary.Entities;

namespace TSAIntegrationLibrary
{
    public class TSA
    {
        /// <summary>
        /// Agrega estampa cronológica al archivo solicitado.
        /// </summary>
        /// <param name="pdfFile">Archivo en base64</param>
        /// <param name="tsaConfig">Configuración de la entidad de TSA</param>
        /// <param name="signatureName">Nombre de estampa</param>
        /// <returns>Archivo en base 64 con estampa incluida</returns>
        public static string AgregarEstampaCronologica(string pdfFile, TSAConfig tsaConfig, string signatureName)
        {
            PdfReader reader = GuardarArchivoTemporal(pdfFile);
            var tsc = new TSAClientBouncyCastle(tsaConfig.Url, tsaConfig.Username, tsaConfig.Password, (new byte[20]).Length, tsaConfig.Algorithm);
            tsc.GetTimeStampToken(new byte[20]);
            byte[] byteArrayPdf;
            using (MemoryStream ms = new MemoryStream())
            {
                PdfSigner ps = new PdfSigner(reader, ms, new StampingProperties().UseAppendMode());
                ps.Timestamp(tsc, signatureName);
                byteArrayPdf = ms.ToArray();
            }
            return Convert.ToBase64String(byteArrayPdf);
        }

        /// <summary>
        /// Almacena provisionalmente el archivo para que pueda ser procesado. 
        /// </summary>
        /// <param name="pdfFile">Archivo en Base64</param>
        /// <returns>Lector de pdf de iText</returns>
        private static PdfReader GuardarArchivoTemporal(string pdfFile)
        {
            string fname = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(fname, Convert.FromBase64String(pdfFile));
            PdfReader reader = new PdfReader(fname);
            if (File.Exists(fname))
            {
                File.Delete(fname);
            }
            return reader;
        }
    }
}
