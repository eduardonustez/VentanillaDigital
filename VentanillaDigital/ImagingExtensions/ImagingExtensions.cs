using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace ImagingExtensions
{
    public static class ImagingExtensions
    {
        public static string ToDataUrl(this Image img)
        {
            return img.ToDataUrl();
        }

        public static Image ToImage(this string dataUrl)
        {
            string base64 = Regex.Replace(dataUrl.Replace("data:", ""), "^.+,", "");
            return Image.Load(Convert.FromBase64String(base64));
        }
    }
}
