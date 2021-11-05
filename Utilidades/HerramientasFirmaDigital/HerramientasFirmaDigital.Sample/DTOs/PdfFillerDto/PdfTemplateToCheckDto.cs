using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasFirmaDigital.Sample.DTOs
{
    public class PdfTemplateToCheckDto
    {
        [Required(ErrorMessage = "Requiere un archivo en formato pdf valido")]
        public IFormFile pdfTemplate { get; set; }
        public string[] fieldsToCheck { get; set; }
    }
}
