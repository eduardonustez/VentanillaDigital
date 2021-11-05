using System;
using System.Collections.Generic;
using System.Text;
using TSAIntegracion.Entities;

namespace TSAIntegracion.Abstraccion
{
    public interface IEstampadorDeTiempo
    {
        byte[] AgregarEstampaCronologica(byte[] pdfFile, ITSAConfig tsaConfig, string signatureName);
    }
}
