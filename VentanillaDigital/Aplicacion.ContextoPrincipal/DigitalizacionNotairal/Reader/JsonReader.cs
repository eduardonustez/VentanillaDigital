using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Reader
{
    public static class JsonReader
    {
        static JsonReader()
        {
            if (Actos == null)
            {
                string path = "/codigosactos.json";
                var codigosactos = ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);
                Actos = JsonConvert.DeserializeObject<Acto>(codigosactos);
            }

            if (Res0826 == null)
            {
                string path = "/res0826.json";
                var res0826 = ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);
                Res0826 = JsonConvert.DeserializeObject<Acto>(res0826);
            }

            if (TiposDocumentos == null)
            {
                string path = "/tiposdocumentos.json";
                var tiposdocumentos = ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);
                TiposDocumentos = JsonConvert.DeserializeObject<Acto>(tiposdocumentos);
            }
        }

        public static string GetCodigoActo(string nombre)
        {
            return Actos.Actos.FirstOrDefault(x => x.Nombre.Equals(nombre)).Codigo;
        }
        public static string GetCodigoRes0826(string nombre)
        {
            return Res0826.Res0826.FirstOrDefault(x => x.Nombre.Equals(nombre)).Codigo;
        }
        public static string GetTipoDocumentoID(string nombre)
        {
            return TiposDocumentos.TiposDocumentos.FirstOrDefault(x => x.Nombre.Equals(nombre)).Idtipodocumento;
        }

        internal static Func<string, string> ReadAllText
        {
            get
            {
                if (readAllTextFunc == null)
                    readAllTextFunc = File.ReadAllText;
                return readAllTextFunc;
            }
            set
            {
                readAllTextFunc = value;
            }
        }

        private static Func<string, string> readAllTextFunc;
        private static readonly Acto Actos;
        private static readonly Acto Res0826;
        private static readonly Acto TiposDocumentos;
    }
}