using HerramientasFirmaDigital.Abstraccion;
using HerramientasFirmaDigital.Sample.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasFirmaDigital.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfFillerController : Controller
    {
        private readonly IPdfFiller _pdfFiller;
        public PdfFillerController(IPdfFiller pdfFiller)
        {
            _pdfFiller = pdfFiller;
        }
        [HttpPost]
        [Route("CheckFields")]
        public async Task<IActionResult> CheckFields([FromForm]PdfTemplateToCheckDto model)
        {
            return Ok(_pdfFiller.CheckFields(FormFileToBytes(model.pdfTemplate), model.fieldsToCheck));
        }
        
        [HttpPost]
        [Route("FillPdf")]
        public async Task<IActionResult> FillPdf(PdfTemplateToFillDto model)
        {
            return File(_pdfFiller.FillPdf(Convert.FromBase64String(model.pdfTemplate),
                model.fields), "application/pdf",
                "filledPdf.pdf");
        }
        [HttpPost]
        [Route("StampPdf")]
        public async Task<IActionResult> StampPdf([FromForm]PdfToStampDto model)
        {
            return File(_pdfFiller.StampPdf(FormFileToBytes(model.pdfDocument),
                        FormFileToBytes(model.image),model.x,model.y,model.newHeight,model.newWidth), "application/pdf",
                "filledPdf.pdf");
        }

        private byte[] FormFileToBytes(IFormFile formFile)
        {
            byte[] fileBytes = null;
            if (formFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }
            return fileBytes;
        }
    }
}
