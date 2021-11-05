using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos
{
    public abstract class Archivo : EntidadBase
    {
        public byte[] Contenido { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamanio { get; set; }
        public string Ruta { get; set; }
        public abstract string UrlPrefix {get;}
        public string ToDataUrl()
        {
            return $"data:{UrlPrefix}/{Extension};base64,{Convert.ToBase64String(Contenido)}";
        }
        public void MapDataUrl(string dataUrl)
        {
            var base64 = dataUrl.Substring(dataUrl.IndexOf(',') + 1);
            Extension = dataUrl.Substring(dataUrl.IndexOf('/') + 1);
            Extension = Extension.Substring(0, Extension.IndexOf(';'));
            Contenido = Convert.FromBase64String(base64);
            Tamanio = Contenido.Length;
        }
        public static TArchivo FromDataUrl<TArchivo>(string dataUrl, string nombre, string ruta)
            where TArchivo: Archivo, new()
        {
            var ret = new TArchivo()
            {
                Nombre = nombre,
                Ruta = ruta
            };
            ret.MapDataUrl(dataUrl);
            return ret;
        }
    }

    public class GrafoNotario : Archivo
    {
        public long GrafoNotarioId { get; set; }
        public override string UrlPrefix => "image";
        public static GrafoNotario FromDataUrl(string dataUrl, string nombre, string ruta)
        {
            return FromDataUrl<GrafoNotario>(dataUrl, nombre, ruta);
        }
    }

    public class SelloNotaria : Archivo
    {
        public long SelloNotariaId { get; set; }
        public override string UrlPrefix => "image";

        public static SelloNotaria FromDataUrl(string dataUrl, string nombre, string ruta)
        {
            return FromDataUrl<SelloNotaria>(dataUrl, nombre, ruta);
        }
    }
}
