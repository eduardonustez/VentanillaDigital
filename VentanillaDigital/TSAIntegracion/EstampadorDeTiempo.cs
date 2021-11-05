using System;
using System.Collections.Generic;
using System.Text;
using TSAIntegracion.Abstraccion;
using TSAIntegracion.Entities;

namespace TSAIntegracion
{
    public class EstampadorDeTiempo : IEstampadorDeTiempo
    {
        public byte[] AgregarEstampaCronologica(byte[] pdfFile, ITSAConfig tsaConfig, string signatureName)
            => TSA.AgregarEstampaCronologica(pdfFile, tsaConfig, signatureName);
    }
}
