using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Parametrizacion
{
    public interface IParametrizacionServicio
    {
        //IparametrizacionServicio
        Task<Categoria[]> ObtenerCategorias();

        Task RegistrarMaquina();
    }
}
