using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Recursos
{
    public interface IRecursosService
    {
        public Task<string> ObtenerRecurso(string url);
    }
}
