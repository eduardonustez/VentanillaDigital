using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.PowerBI
{
    public interface IReporteService
    {
        EmbedParams ObtenerReporteEmbed(string tipoReporte, Guid filtroNotaria, [Optional] Guid additionalDatasetId);
    }
}
