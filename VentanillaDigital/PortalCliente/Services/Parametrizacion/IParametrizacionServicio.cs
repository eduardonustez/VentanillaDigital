using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Parametrizacion
{
    public interface IParametrizacionServicio
    {
        //IparametrizacionServicio
        Task<Categoria[]> ObtenerCategorias();

        Task RegistrarMaquina();
        Task ValidarEstadoCaptorHuella(bool primerIntento = true);
    }
}
