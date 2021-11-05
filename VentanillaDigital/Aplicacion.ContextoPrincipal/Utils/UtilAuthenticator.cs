using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;

namespace Aplicacion.ContextoPrincipal.Utils
{
    public class UtilAuthenticator
    {

        public enum responseSerial : short
        {
            noCambio = 1,
            error = -1,
            cambio = 0
        }

        RNGCryptoServiceProvider rnd;

        protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        //largo del pin
        private int intervalLength = 40;
        //segundos para cambio del pin
        private int pinCodeLength = 6;
        private int pinModulo = (int)Math.Pow(10, 6);//(int)Math.Pow(10, pinCodeLength);

        private byte[] randomBytes = new byte[10];

        public string createUserToken(string userLogin)
        {
            rnd = new RNGCryptoServiceProvider();

            pinModulo = (int)Math.Pow(10, pinCodeLength);

            rnd.GetBytes(randomBytes);

            return Transcoder.Base32Encode(randomBytes);
        }

        public System.Drawing.Image GenerateImage(string email, string token)
        {
            //string randomString = CreativeCommons.Transcoder.Base32Encode(randomBytes);
            byte[] decode = Transcoder.Base32Decode(token);
            randomBytes = decode;
            string ProvisionUrl = UrlEncode(String.Format("otpauth://totp/{0}?secret={1}", email, token));
            string url = String.Format("http://chart.apis.google.com/chart?cht=qr&chs={0}x{1}&chl={2}", 280, 230, ProvisionUrl);

            WebClient wc = new WebClient();
            //wc.Proxy.Credentials = new NetworkCredential("OLIMPIAIT\\sospina", "Ubuntu.1");
            var data = wc.DownloadData(url);

            using (var imageStream = new MemoryStream(data))
            {
                return new System.Drawing.Bitmap(imageStream);
            }
        }

        public bool AutenticarOtp(string pin, string token)
        {
            bool response = false;
            byte[] decode = Transcoder.Base32Decode(token);
            //var aux = GeneratePin(decode);
            if (GeneratePin(decode, 0).Equals(pin))//por la registraduria solo validar el pin actual
                response = true;
            //else
            //    if (GeneratePin(decode, 1).Equals(pin))
            //    response = true;
            //else
            //    if (GeneratePin(decode, -1).Equals(pin))
            //    response = true;

            return response;
        }

        private string GeneratePin(byte[] otp, int interval)
        {
            return generateResponseCode(getInterval(interval), otp);
        }

        private String generateResponseCode(long challenge, byte[] randomBytes)
        {
            HMACSHA1 myHmac = new HMACSHA1(randomBytes);
            myHmac.Initialize();

            byte[] value = BitConverter.GetBytes(challenge);
            Array.Reverse(value); //reverses the challenge array due to differences in c# vs java
            myHmac.ComputeHash(value);
            byte[] hash = myHmac.Hash;
            int offset = hash[hash.Length - 1] & 0xF;
            byte[] SelectedFourBytes = new byte[4];
            //selected bytes are actually reversed due to c# again, thus the weird stuff here
            SelectedFourBytes[0] = hash[offset];
            SelectedFourBytes[1] = hash[offset + 1];
            SelectedFourBytes[2] = hash[offset + 2];
            SelectedFourBytes[3] = hash[offset + 3];
            Array.Reverse(SelectedFourBytes);
            int finalInt = BitConverter.ToInt32(SelectedFourBytes, 0);
            int truncatedHash = finalInt & 0x7FFFFFFF; //remove the most significant bit for interoperability as per HMAC standards
            int pinValue = truncatedHash % pinModulo; //generate 10^d digits where d is the number of digits
            return padOutput(pinValue);
        }

        private String padOutput(int value)
        {
            String result = value.ToString();
            for (int i = result.Length; i < pinCodeLength; i++)
            {
                result = "0" + result;
            }
            return result;
        }

        protected string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        private long getInterval(int interval)
        {
            TimeSpan TS = DateTime.UtcNow.AddSeconds(intervalLength * interval) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long currentTimeSeconds = (long)Math.Floor(TS.TotalSeconds);
            long currentInterval = currentTimeSeconds / intervalLength; // 40 Seconds
            return currentInterval;
        }

        //clase para pasar de base32 a byte[]
        public class Transcoder
        {
            private const int IN_BYTE_SIZE = 8;
            private const int OUT_BYTE_SIZE = 5;
            private static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
            private static string alphaux = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";


            public static string Base32Encode(byte[] data)
            {
                int i = 0, index = 0, digit = 0;
                int current_byte, next_byte;
                StringBuilder result = new StringBuilder((data.Length + 7) * IN_BYTE_SIZE / OUT_BYTE_SIZE);

                while (i < data.Length)
                {
                    current_byte = (data[i] >= 0) ? data[i] : (data[i] + 256); // Unsign

                    /* Is the current digit going to span a byte boundary? */
                    if (index > (IN_BYTE_SIZE - OUT_BYTE_SIZE))
                    {
                        if ((i + 1) < data.Length)
                            next_byte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                        else
                            next_byte = 0;

                        digit = current_byte & (0xFF >> index);
                        index = (index + OUT_BYTE_SIZE) % IN_BYTE_SIZE;
                        digit <<= index;
                        digit |= next_byte >> (IN_BYTE_SIZE - index);
                        i++;
                    }
                    else
                    {
                        digit = (current_byte >> (IN_BYTE_SIZE - (index + OUT_BYTE_SIZE))) & 0x1F;
                        index = (index + OUT_BYTE_SIZE) % IN_BYTE_SIZE;
                        if (index == 0)
                            i++;
                    }
                    result.Append(alphabet[digit]);
                }

                return result.ToString();
            }

            /// <summary>
            /// Convert base32 string to array of bytes
            /// </summary>
            /// <param name="base32String">Base32 string to convert</param>
            /// <returns>Returns a byte array converted from the string</returns>
            internal static byte[] Base32Decode(string base32String)
            {
                // Check if string is null
                if (base32String == null)
                {
                    return null;
                }
                // Check if empty
                else if (base32String == string.Empty)
                {
                    return new byte[0];
                }

                // Convert to upper-case
                string base32StringUpperCase = base32String.ToUpperInvariant();

                // Prepare output byte array
                byte[] outputBytes = new byte[base32StringUpperCase.Length * OUT_BYTE_SIZE / IN_BYTE_SIZE];

                // Check the size
                if (outputBytes.Length == 0)
                {
                    throw new ArgumentException("Specified string is not valid Base32 format because it doesn't have enough data to construct a complete byte array");
                }

                // Position in the string
                int base32Position = 0;

                // Offset inside the character in the string
                int base32SubPosition = 0;

                // Position within outputBytes array
                int outputBytePosition = 0;

                // The number of bits filled in the current output byte
                int outputByteSubPosition = 0;

                // Normally we would iterate on the input array but in this case we actually iterate on the output array
                // We do it because output array doesn't have overflow bits, while input does and it will cause output array overflow if we don't stop in time
                while (outputBytePosition < outputBytes.Length)
                {
                    // Look up current character in the dictionary to convert it to byte
                    int currentBase32Byte = alphaux.IndexOf(base32StringUpperCase[base32Position]);

                    // Check if found
                    if (currentBase32Byte < 0)
                    {
                        throw new ArgumentException(string.Format("Specified string is not valid Base32 format because character \"{0}\" does not exist in Base32 alphabet", base32String[base32Position]));
                    }

                    // Calculate the number of bits we can extract out of current input character to fill missing bits in the output byte
                    int bitsAvailableInByte = Math.Min(OUT_BYTE_SIZE - base32SubPosition, IN_BYTE_SIZE - outputByteSubPosition);

                    // Make space in the output byte
                    outputBytes[outputBytePosition] <<= bitsAvailableInByte;

                    // Extract the part of the input character and move it to the output byte
                    outputBytes[outputBytePosition] |= (byte)(currentBase32Byte >> (OUT_BYTE_SIZE - (base32SubPosition + bitsAvailableInByte)));

                    // Update current sub-byte position
                    outputByteSubPosition += bitsAvailableInByte;

                    // Check overflow
                    if (outputByteSubPosition >= IN_BYTE_SIZE)
                    {
                        // Move to the next byte
                        outputBytePosition++;
                        outputByteSubPosition = 0;
                    }

                    // Update current base32 byte completion
                    base32SubPosition += bitsAvailableInByte;

                    // Check overflow or end of input array
                    if (base32SubPosition >= OUT_BYTE_SIZE)
                    {
                        // Move to the next character
                        base32Position++;
                        base32SubPosition = 0;
                    }
                }

                return outputBytes;
            }
        }
    }
}
