using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ICategoriaServicio:IDisposable
    {
        Task<IEnumerable<CategoriaReturnDTO>> ObtenerCategorias();

        Task<IEnumerable<CategoriaReturnDTO>> ObtenerCategoriaNotariaVirtual();
    }
}
