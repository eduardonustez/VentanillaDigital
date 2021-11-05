using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasFirmaDigital
{
    public class DatosFirma
    {
        public string SerialCertificado { get; set; }
        public string Titulo { get; set; }
        public string UrlQR { get; set; }
        public string TextoFirma { get; set; }
        public byte[] Grafo { get; set; }
        public string FormatoGrafo { get; set; }
        public byte[] Sello { get; set; }
        public string FormatoSello { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
    }
}
