using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasFirmaDigital.Interno.Abstraccion
{
    internal interface IGeneradorPaginaFirma
    {
        byte[] GenerarPagina(DatosFirma datosFirma);
    }
}
