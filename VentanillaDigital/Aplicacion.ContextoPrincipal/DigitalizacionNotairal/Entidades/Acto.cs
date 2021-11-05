using System.Collections.Generic;

namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades
{
    public class Acto
    {
        public List<BodyActo> Actos
        {
            get;
            set;
        }

        public List<BodyActo> Res0826
        {
            get;
            set;
        }

        public List<TipoDocumento> TiposDocumentos
        {
            get;
            set;
        }

        public class BodyActo
        {
            public string Nombre
            {
                get;
                set;
            }

            public string Codigo
            {
                get;
                set;
            }
        }

        public class TipoDocumento
        {
            public string Nombre
            {
                get;
                set;
            }

            public string Idtipodocumento
            {
                get;
                set;
            }
        }
    }
}