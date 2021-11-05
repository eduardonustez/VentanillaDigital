using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasFirmaDigital.Sample.DTOs
{
    public class PdfToStampDto
    {
        [Required(ErrorMessage = "Requiere un archivo en formato pdf valido")]
        public IFormFile pdfDocument { get; set; }
        [Required(ErrorMessage = "Requiere un archivo en formato png/jpg valido")]
        public IFormFile image { get; set; }
        [DefaultValue(100)]
        public float x { get; set; }
        [DefaultValue(100)]
        public float y { get; set; }
        [DefaultValue(-1)]
        public float newWidth { get; set; }
        [DefaultValue(-1)]
        public float newHeight { get; set; }
    }
}
