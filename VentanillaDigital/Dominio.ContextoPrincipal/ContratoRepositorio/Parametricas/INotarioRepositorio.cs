using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal.Models;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface INotarioRepositorio : IRepositorioBase<Notario>, IDisposable
    {
        Task<bool> NotariaActiva(long NotariaId);
        Task<int> ActualizarNotario(Notario Notario);
        Task<RespuestaProcedimientoViewModel> ObtenerNotariosPaginado(DefinicionFiltro definicionFiltro);
        Task<Notario> ObtenerNotarioPrincipal(long notariaId, string[] includes = null);        
    }
}
