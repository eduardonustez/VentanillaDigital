using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.ExtensionMethods
{
    public static class HtmlConvertExtensions
    {
        public static string GetHtmlStringFromFormFile(this IFormFile file)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    using (MemoryStream htmlStream = new MemoryStream(fileBytes))
                    {
                        htmlDocument.Load(htmlStream);
                    }

                }
            }
            return htmlDocument.Text;
        }
    }
}
