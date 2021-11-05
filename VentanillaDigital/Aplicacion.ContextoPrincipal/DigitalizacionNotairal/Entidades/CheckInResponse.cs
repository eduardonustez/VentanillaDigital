using System;

namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades
{
    public class CheckInResponse
    {
        public string AccessCode
        {
            get;
            set;
        }

        public string CheckInStatus
        {
            get;
            set;
        }

        public string Extension
        {
            get;
            set;
        }

        public Log Log
        {
            get;
            set;
        }

        public string MimeType
        {
            get;
            set;
        }

        public string PublicFileLink
        {
            get;
            set;
        }

        public DateTime PublicFileLinkValidTo
        {
            get;
            set;
        }

        public string Size
        {
            get;
            set;
        }
    }
}