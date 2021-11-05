using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Encriptacion
{
    public static class CifradoSHA512
    {
        public static string Cifrar(string text)
        {
            UnicodeEncoding converter = new UnicodeEncoding();
            byte[] input = converter.GetBytes(text);
            List<String> output = new List<String>();

            SHA512Managed sha512 = new SHA512Managed();
            return Convert.ToBase64String(sha512.ComputeHash(input));
        }
     
        public static bool CompararTexto(string textoReferencia, string textocifrado)
        {
            string textoReferenciaCifrado = Cifrar(textoReferencia);
            byte[] arrayIntroducido = Convert.FromBase64String(textoReferenciaCifrado);
            byte[] arrayAComparar = Convert.FromBase64String(textocifrado);
            return CompararBytes(arrayIntroducido, arrayAComparar);

        }

        private static bool CompararBytes(byte[] ar1, byte[] ar2)
        {
            if (ar1.Length != ar2.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < ar1.Length; i++)
                {
                    if (!(ar1[i].Equals(ar2[i])))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
