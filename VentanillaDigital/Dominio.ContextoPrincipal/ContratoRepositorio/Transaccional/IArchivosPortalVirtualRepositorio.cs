using Dominio.ContextoPrincipal.Entidad;
using Dominio.Nucleo;
using System;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IArchivosPortalVirtualRepositorio : IRepositorioBase<ArchivosPortalVirtual>, IDisposable
    {
        Task<string> ObtenerArchivoPortalVirtual(long ArchivoPortalVirtualId);
    }
}
