using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Infraestructura.KeyManager.Models;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.XMP.Impl;
using iText.Signatures;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using TSAIntegracion.Entities;
using Path = System.IO.Path;

namespace TSAIntegracion
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
        public static string AgregarEstampaCronologicaString(
            string pdfFile, ITSAConfig tsaConfig, string signatureName)
                => Convert.ToBase64String(
                    AgregarEstampaCronologica(
                        Convert.FromBase64String(pdfFile), 
                        tsaConfig, 
                        signatureName));
        /// <summary>
        /// Agrega estampa cronológica al archivo solicitado.
        /// </summary>
        /// <param name="pdfFile">Archivo en byte[]</param>
        /// <param name="tsaConfig">Configuración de la entidad de TSA</param>
        /// <param name="signatureName">Nombre de estampa</param>
        /// <returns>Archivo en base64 con estampa incluida</returns>
        public static string AgregarEstampaCronologicaString(
            byte[] pdfFile, 
            ITSAConfig tsaConfig, 
            string signatureName)
                => Convert.ToBase64String(
                    AgregarEstampaCronologica(
                        pdfFile, 
                        tsaConfig, 
                        signatureName));
        /// <summary>
        /// Agrega estampa cronológica al archivo solicitado.
        /// </summary>
        /// <param name="pdfFile">Archivo en base64</param>
        /// <param name="tsaConfig">Configuración de la entidad de TSA</param>
        /// <param name="signatureName">Nombre de estampa</param>
        /// <returns>Archivo en byte[] con estampa incluida</returns>
        public static byte[] AgregarEstampaCronologica(
            string pdfFile,
            ITSAConfig tsaConfig,
            string signatureName)
                => AgregarEstampaCronologica(
                        Convert.FromBase64String(pdfFile),
                        tsaConfig,
                        signatureName);
        public static string AgregarEstampaCronologicaString(
          string pdfFile, ITSAConfig tsaConfig, string signatureName,
          IExternalSignature pks, X509Certificate2 publicKey)
              => Convert.ToBase64String(
                  AgregarEstampaCronologica(
                      Convert.FromBase64String(pdfFile),
                      tsaConfig,
                      signatureName, pks, publicKey));
        public static string AgregarEstampaCronologicaSinFirma(string pdfFile, 
            ITSAConfig tsaConfig, string signatureName)
                => Convert.ToBase64String(
                    AgregarEstampaCronologicaSinFirma(
                        Convert.FromBase64String(pdfFile),
                        tsaConfig,
                        signatureName));
        /// <summary>
        /// Agrega estampa cronológica al archivo solicitado.
        /// </summary>
        /// <param name="pdfFile">Archivo en byte[]</param>
        /// <param name="tsaConfig">Configuración de la entidad de TSA</param>
        /// <param name="signatureName">Nombre de estampa</param>
        /// <returns>Archivo en byte[] con estampa incluida</returns>
        public static byte[] AgregarEstampaCronologica(byte[] pdfFile, ITSAConfig tsaConfig, string signatureName)
        {
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[1];

            X509Certificate2 cert1 = new X509Certificate2();

            //IOcspClient ocspClient = new OcspClientBouncyCastle(null);

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificateCollection = store.Certificates
                .Find(X509FindType.FindBySerialNumber, tsaConfig.CertificateSerialNumber, false);
            byte[] byteArrayPdf = new byte[] { };
            if (certificateCollection != null && certificateCollection.Count > 0)
            {
                cert1 = certificateCollection[0];
            }
            else
            {
                return pdfFile;
            }


            var cert = DotNetUtilities.FromX509Certificate(cert1);
            //var keypair1 = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(pk1);
            //var pktest = keypair1.Private;
            X509CertificateParser cp = new X509CertificateParser();
            chain = new Org.BouncyCastle.X509.X509Certificate[] { cert };

            using (PdfReader reader = new PdfReader(new MemoryStream(pdfFile)))
            {
                ITSAClient tsc = new TSAClientBouncyCastle(tsaConfig.Url, tsaConfig.Username, tsaConfig.Password, (new byte[20]).Length, tsaConfig.Algorithm);


                using (MemoryStream ms = new MemoryStream())
                {
                    PdfSigner ps = new PdfSigner(reader, ms, new StampingProperties());
                    //PdfSignatureAppearance appearance = ps.GetSignatureAppearance();
                    //appearance
                    //    .SetReason(tsaConfig.Reason)
                    //    .SetLocation(tsaConfig.Location)
                    //    .SetReuseAppearance(false)
                    //    .SetPageRect(rect)
                    //    .SetPageNumber(1);
                    ps.SetFieldName(signatureName);
                    IExternalSignature pks = new X509Certificate2Signature(cert1, tsaConfig.Algorithm);
                    ps.SignDetached(pks, chain, null, null, tsc, 0, PdfSigner.CryptoStandard.CMS);
                    byteArrayPdf = ms.ToArray();
                }
                return byteArrayPdf;
            }
        }
        public static byte[] AgregarEstampaCronologica(byte[] pdfFile, ITSAConfig tsaConfig, 
            string signatureName, IExternalSignature pks,X509Certificate2 publicKey)
        {
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[1];
            //chain = new Org.BouncyCastle.X509.X509Certificate[] { pks._Ce };
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            byte[] byteArrayPdf = new byte[] { };
            var cert = DotNetUtilities.FromX509Certificate(publicKey);
            
            
            chain = new Org.BouncyCastle.X509.X509Certificate[] { cert };
            using (PdfReader reader = new PdfReader(new MemoryStream(pdfFile)))
            {
                ITSAClient tsc = new TSAClientBouncyCastle(tsaConfig.Url, tsaConfig.Username, tsaConfig.Password, (new byte[20]).Length, tsaConfig.Algorithm);

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfSigner ps = new PdfSigner(reader, ms, new StampingProperties());
                    ps.SetFieldName(signatureName);
                    try
                    {
                        ps.SignDetached(pks, chain, null, null, tsc, 0, PdfSigner.CryptoStandard.CMS);
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    byteArrayPdf = ms.ToArray();
                }
                return byteArrayPdf;
            }
        }
        public static byte[] AgregarEstampaCronologicaSinFirma(byte[] pdfFileSigned, 
            ITSAConfig tsaConfig, string signatureName)
        {
            byte[] byteArrayPdf = new byte[] { };
            using (PdfReader reader = new PdfReader(new MemoryStream(pdfFileSigned)))
            {
                ITSAClient tsc = new TSAClientBouncyCastle(tsaConfig.Url, tsaConfig.Username, tsaConfig.Password);

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfSigner ps = new PdfSigner(reader, ms, new StampingProperties());
                    ps.SetFieldName(signatureName);
                    ps.Timestamp(tsc, signatureName);
                    byteArrayPdf = ms.ToArray();
                }
                return byteArrayPdf;
            }
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
            //if (File.Exists(fname))
            //{
            //    File.Delete(fname);
            //}
            return reader;
        }
    }
}
