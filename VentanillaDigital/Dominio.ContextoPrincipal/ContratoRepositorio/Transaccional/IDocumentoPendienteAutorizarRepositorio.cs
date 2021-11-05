using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional
{
    public interface IDocumentoPendienteAutorizarRepositorio : IRepositorioBase<DocumentoPendienteAutorizar>, IDisposable
    {
        Task<IEnumerable<DocumentoPendienteAutorizar>> ObtenerProximas(int cantidad);
        Task<List<long>> ObtenerProximasSpAsync(int cantidad);
    }
}
