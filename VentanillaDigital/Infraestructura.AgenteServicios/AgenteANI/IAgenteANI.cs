using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.AgenteServicios.AgenteANI
{
    public interface IAgenteANI
    {
        Task<ANIResponseModel> ValidarPersona(int tipoDocumento,string documento);
    }
}
