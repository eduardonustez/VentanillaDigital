using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace GeneradorQR
{
    public class GeneradorQR : IGeneradorQR
    {
        public string StringToQR(string qrString)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrString, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            var img = BitmapToBytesCode(qrCodeImage);

            var resul = string.Concat("data:image/png;base64,", Convert.ToBase64String(img));
            return resul;

        }
        private static byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
