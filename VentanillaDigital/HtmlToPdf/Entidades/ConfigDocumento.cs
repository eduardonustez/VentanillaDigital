using System.Collections.Generic;

namespace HtmlToPdf.Entidades
{
    public class ConfigDocumento
    {
        public int BrowserWidth { get; set; }
        public float LeftMargin { get; set; }
        public float RightMargin { get; set; }
        public float TopMargin { get; set; }
        public float BottomMargin { get; set; }
        public bool UseBorder { get; set; }

        public ConfigDocumento()
        {
            //Default values to Acta Notarial
            BrowserWidth = 776;
            LeftMargin = 14;
            RightMargin = 14;
            TopMargin = 14;
            BottomMargin = 14;
            UseBorder = true;
        }
    }
}
