using Dominio.ContextoPrincipal.Entidad.Log;
using Dominio.Nucleo;
using System;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Log
{
    public interface ILogTramitePortalVirtualRepositorio : IRepositorioBase<LogTramitePortalVirtual>, IDisposable
    {
    }
}
