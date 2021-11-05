using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.ExtensionMethods
{
    public static class ConvertExtensions
    {
        public static string GetContentTypeAndBase64FromFormFile(this IFormFile file)
        {
            string base64 = "";
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    base64 = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                }
            }
            return $"data:{file.ContentType};base64,{base64}";
        }

        public static string ConvertirALetras(this DateTime fecha)
        {
            var dia = NumeroALetras(fecha.Day);

            return $"{dia} ({fecha.Day}) de {Mes(fecha.Month)} de {NumeroALetras(fecha.Year)} ({fecha.Year})";
        }

        public static string Mes(this int mes)
        {
            switch (mes)
            {
                case 1: return "enero";
                case 2: return "febrero";
                case 3: return "marzo";
                case 4: return "abril";
                case 5: return "mayo";
                case 6: return "junio";
                case 7: return "julio";
                case 8: return "agosto";
                case 9: return "septiempre";
                case 10: return "octubre";
                case 11: return "noviembre";
                case 12: return "diciembre";
                default:
                    return "";
            }
            //=> mes switch
            //   {
            //       1 => "enero",
            //       2 => "febrero",
            //       3 => "marzo",
            //       4 => "abril",
            //       5 => "mayo",
            //       6 => "junio",
            //       7 => "julio",
            //       8 => "agosto",
            //       9 => "septiempre",
            //       10 => "octubre",
            //       11 => "noviembre",
            //       12 => "diciembre",
            //       _ => ""
            //   };
        }

        public static string NumeroALetras(long value)
        {
            var entero = value; // Convert.ToInt64(Math.Truncate(numberAsString));
            var res = NumeroALetras(Convert.ToDouble(entero));
            return res;
        }

        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "cero";
            else if (value == 1) num2Text = "uno";
            else if (value == 2) num2Text = "dos";
            else if (value == 3) num2Text = "tres";
            else if (value == 4) num2Text = "cuatro";
            else if (value == 5) num2Text = "cinco";
            else if (value == 6) num2Text = "seis";
            else if (value == 7) num2Text = "siete";
            else if (value == 8) num2Text = "ocho";
            else if (value == 9) num2Text = "nueve";
            else if (value == 10) num2Text = "diez";
            else if (value == 11) num2Text = "once";
            else if (value == 12) num2Text = "doce";
            else if (value == 13) num2Text = "trece";
            else if (value == 14) num2Text = "catorce";
            else if (value == 15) num2Text = "quince";
            else if (value < 20) num2Text = "cieci" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "veinte";
            else if (value < 30) num2Text = "veinti" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "treinta";
            else if (value == 40) num2Text = "cuarenta";
            else if (value == 50) num2Text = "cincuenta";
            else if (value == 60) num2Text = "sesenta";
            else if (value == 70) num2Text = "setenta";
            else if (value == 80) num2Text = "ochenca";
            else if (value == 90) num2Text = "noventa";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "cien";
            else if (value < 200) num2Text = "ciento " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "quinientos";
            else if (value == 700) num2Text = "setecientos";
            else if (value == 900) num2Text = "novecientos";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "mil";
            else if (value < 2000) num2Text = "mil " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " mil";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "un millon";
            }
            else if (value < 2000000)
            {
                num2Text = "un millon " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " millones ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "un billon";
            else if (value < 2000000000000) num2Text = "un billon " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " billones";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }
    }
}
