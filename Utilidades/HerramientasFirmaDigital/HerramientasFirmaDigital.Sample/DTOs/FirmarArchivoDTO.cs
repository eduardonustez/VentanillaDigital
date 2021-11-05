using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasFirmaDigital.Sample.DTOs
{
    public class FirmarArchivoDTO
    {
        public IFormFile DocumentoOriginal { get; set; }
        public string SerialCertificado { get; set; }
        public string Titulo { get; set; }
        public string UrlQR { get; set; }
        public string TextoFirma { get; set; }
        public IFormFile Grafo { get; set; }
        public IFormFile Sello { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
    }
}
