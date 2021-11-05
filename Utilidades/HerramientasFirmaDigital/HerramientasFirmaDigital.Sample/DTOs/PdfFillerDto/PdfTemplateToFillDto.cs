using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HerramientasFirmaDigital.Sample.DTOs
{
    public class PdfTemplateToFillDto
    {
        [Required(ErrorMessage ="Requiere un archivo base64String valido")]
        public string pdfTemplate { get; set; }
        public Dictionary<string,string> fields { get; set; }
    }
}
