using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace Infraestructura.Transversal
{
    public static class Cifrador
    {
        /// <summary>
        /// Encripta un string
        /// </summary>
        /// <param name="plainString">texto a cifrar</param>
        /// <param name="llave">token con el que se cifra</returns>
        /// <returns>string texto cifrado</returns>
        public static string CifradoVariable(string plainString, string llave)
        {
            byte[] bytesTextToCipher = Encoding.UTF8.GetBytes(plainString);
            byte[] encryptedBytes = null;
            byte[] bytesSalt = null;
            byte[] bytesIV = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged encryptor = new RijndaelManaged())
                {
                    // extract salt (first 16 bytes)
                    bytesSalt = ObtenerSalt(16);
                    // extract iv (next 16 bytes)
                    bytesIV = ObtenerSalt(16);
                    // the rest is encrypted data
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(llave, bytesSalt, 100);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.Padding = PaddingMode.PKCS7;
                    encryptor.Mode = CipherMode.CBC;
                    encryptor.IV = bytesIV;

                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesTextToCipher, 0, bytesTextToCipher.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return Convert.ToBase64String(bytesSalt.Concat(bytesIV).Concat(encryptedBytes).ToArray());
        }

        /// <summary>
        /// Desencripta un string, ejemplo tomado de https://stackoverflow.com/questions/50000746/encrypting-in-angular-and-decrypt-on-c-sharp
        /// </summary>
        /// <param name="encryptedString">texto cifrado</param>
        /// <param name="llave">token con el que se cifra</returns>
        /// <returns>string texto sin cifrar</returns>
        public static string DescifradoVariable(string encryptedString, string llave)
        {
            byte[] cipherBytes = Convert.FromBase64String(encryptedString);
            using (Aes encryptor = Aes.Create())
            {
                // extract salt (first 16 bytes)
                var salt = cipherBytes.Take(16).ToArray();
                // extract iv (next 16 bytes)
                var iv = cipherBytes.Skip(16).Take(16).ToArray();
                // the rest is encrypted data
                var encrypted = cipherBytes.Skip(32).ToArray();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(llave, salt, 100);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.Padding = PaddingMode.PKCS7;
                encryptor.Mode = CipherMode.CBC;
                encryptor.IV = iv;
                // you need to decrypt this way, not the way in your question
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene una estructura serializada { "prop1": "val1", "prop2": "val2" ... }
        /// </summary>
        /// <returns>string objeto serializado formato JSON</returns>
        public static string ObtenerObjetoUnico()
        {
            var keyObject = new ExpandoObject() as IDictionary<string, Object>;
            var key = Guid.NewGuid().ToString();

            foreach (var property in key.Split('-'))
            {
                keyObject.Add($"{property}{ObtenerLlaveUnica((new Random()).Next(0, 3))}", ObtenerLlaveUnica((new Random()).Next(3, 62)));
            }

            return JsonConvert.SerializeObject(keyObject.ToDictionary(x => x.Key, x => x.Value));
        }

        /// <summary>
        /// Obtiene un token string desde un objeto con una estructura serializada { "prop1": "val1", "prop2": "val2" ... }
        /// </summary>
        /// <param name="keyObject">objecto con una estructura serializada { "prop1": "val1", "prop2": "val2" ... }</param>
        /// <returns>string obtiene el token generado a partir del objeto específico</returns>
        public static string ObtenerTokenDesdeObjeto(dynamic keyObjectSerialized)
        {
            dynamic keyObject = JsonConvert.DeserializeObject<IDictionary<string, object>>(keyObjectSerialized);
            var dictionarity = ((IDictionary<string, object>)keyObject);
            return String.Join(string.Empty, dictionarity.Select(t => $"{t.Key}{t.Value.ToString().Substring(0, 3)}"));
        }

        /// <summary>
        /// Arreglo de bytes al azar
        /// </summary>
        /// <param name="maximumSaltLength">cantidad máxima de caracteres</param>
        /// <returns>byte[] arrglo de bytes</returns>
        private static byte[] ObtenerSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Obtiene un número de carácteres en random
        /// </summary>
        /// <param name="maxSize">tamaño de la toma de carácteres</returns>
        private static string ObtenerLlaveUnica(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Proveedor de cifrado estatico
        /// </summary>
        /// <param name="llave">token con el que se cifra</param>
        /// <returns>Proveedor de cifrado estático</returns>
        private static TripleDESCryptoServiceProvider ObtenerProveedorCifradoEstatico(string llave)
        {
            var md5 = new MD5CryptoServiceProvider();
            var key = md5.ComputeHash(Encoding.UTF8.GetBytes(llave));
            return new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
        }

        /// <summary>
        /// Realiza un crifrado estático
        /// </summary>
        /// <param name="textoACifrar">string a cifrar</param>
        /// <param name="llave">token con el que se cifra</param>
        /// <returns>Texto cifrado</returns>
        public static string CifradoEstatico(string textoACifrar, string llave)
        {
            var data = Encoding.UTF8.GetBytes(textoACifrar);
            var tripleDes = ObtenerProveedorCifradoEstatico(llave);
            var transform = tripleDes.CreateEncryptor();
            var resultsByteArray = transform.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(resultsByteArray);
        }

        /// <summary>
        /// Realiza un descifrado estático
        /// </summary>
        /// <param name="textoCifrado">string a descifrar</param>
        /// <param name="llave">token con el que se cifra</param>
        /// <returns>Texto descifrado</returns>
        public static string DescifradoEstatico(string textoCifrado, string llave)
        {
            var data = Convert.FromBase64String(textoCifrado);
            var tripleDes = ObtenerProveedorCifradoEstatico(llave);
            var transform = tripleDes.CreateDecryptor();
            var resultsByteArray = transform.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(resultsByteArray);
        }
    }
}
