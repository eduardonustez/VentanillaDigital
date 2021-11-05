using Infraestructura.Transversal.Adaptador;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Helper
{
    public static class Utilities
    {
        public static string ObtenerBase64Documento(IFormFile file)
        {
            string response = string.Empty;
            using (var ms = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(ms);
                var fileBytes = ms.ToArray();
                response = Convert.ToBase64String(fileBytes);
            }
            return response;
        }
    }
}
