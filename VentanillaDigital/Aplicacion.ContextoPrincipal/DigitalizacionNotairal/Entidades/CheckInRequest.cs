
namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades
{
    public class CheckInRequest
    {
        public string DocumentType
        {
            get;
            set;
        }

        public string CheckInType
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string FileContent
        {
            get;
            set;
        }

        public Tag[] Tags
        {
            get;
            set;
        }
        public string ApiUser { get; set; }
        public string ApiKey { get; set; }
    }
}