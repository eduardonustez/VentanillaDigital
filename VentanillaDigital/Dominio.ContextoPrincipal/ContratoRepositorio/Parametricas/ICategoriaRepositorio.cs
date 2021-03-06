using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface ICategoriaRepositorio : IRepositorioBase<Categoria>, IDisposable
    {
        
    }
}
