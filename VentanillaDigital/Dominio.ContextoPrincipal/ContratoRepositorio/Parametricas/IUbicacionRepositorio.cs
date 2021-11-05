using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IUbicacionRepositorio : IRepositorioBase<Ubicacion>, IDisposable
    {
       
    }
}
