using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Wacom.Models
{
    public class SignatureSize
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public float LineWidth { get; set; }
        public SignatureSize(int height,int width,float lineWidth)
        {
            this.Height = height;
            this.Width=width;
            this.LineWidth=lineWidth;
        }
    }
}
