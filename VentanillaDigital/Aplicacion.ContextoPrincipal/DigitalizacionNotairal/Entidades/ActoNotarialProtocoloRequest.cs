
namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades
{
    public class ActoNotarialProtocoloRequest
    {
        public string IdNotaria
        {
            get;
            set;
        }

        public string AutorizacionAutenticacion
        {
            get;
            set;
        }

        public int TipoDeDocumento
        {
            get;
            set;
        }

        public int Codigo_acto
        {
            get;
            set;
        }

        public string Fecha_acto //Año-mes-dia
        {
            get;
            set;
        }

        public string Cuandi
        {
            get;
            set;
        }

        public string Consecutivo
        {
            get;
            set;
        }

        public string MatriculaRelacionada
        {
            get;
            set;
        }

        public string Factura_recibo
        {
            get;
            set;
        }

        public string Palabras_claves
        {
            get;
            set;
        }

        public string Datos_interesados
        {
            get;
            set;
        }
    }
}