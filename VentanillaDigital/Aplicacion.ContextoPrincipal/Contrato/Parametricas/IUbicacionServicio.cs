using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad;
using Infraestructura.Transversal.HandlingError;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface IUbicacionServicio:IDisposable
    {
        Task<IEnumerable<UbicacionReturnDTO>> GetDepartamentos();
        Task<IEnumerable<UbicacionReturnDTO>> GetCiudades(int paisId);
     

    }
}
