using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IArchivoRepositorio : IRepositorioBase<Archivo>, IDisposable
    {
        
    }
}
