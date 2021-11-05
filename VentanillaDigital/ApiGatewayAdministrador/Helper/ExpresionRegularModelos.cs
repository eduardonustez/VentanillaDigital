using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Helper
{
    public static class ExpresionRegularModelos
    {
        public const string NumeroDocumento = @"^([0-9]{6,11})$";
        public const string NombreDocumento = @"^[\w,\s-]+\.[A-Za-z]{3,4}$";
        public const string mimeTypePDF = "application/pdf";
        public const string mimeTypeImagenJpeg = "image/jpeg";
        public const string mimeTypeImagenJpg = "image/jpg";
        public const string mimeTypeImagenPng = "image/png";
        public const string mimeTypeImagenBmp = "image/bmp";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

        public const string MensajePassword = "Las contraseñas deben tener al menos 8 caracteres y contener al menos un carácter de cada tipo: mayúscula (A-Z), minúscula (a-z), número (0-9) y un caracter especial (e.j. !@#$%,^&*)";

        public const string PasswordDiferentes = "Las contraseñas no coinciden";
    }
}
