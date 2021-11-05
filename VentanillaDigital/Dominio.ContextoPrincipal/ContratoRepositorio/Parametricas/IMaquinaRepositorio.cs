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
    public interface IMaquinaRepositorio : IRepositorioBase<Maquina>, IDisposable
    {
        Task<bool> NotariaActiva(long NotariaId);
        Task<int> ActualizarMaquina(Maquina Maquina);
        Task<RespuestaProcedimientoViewModel> ObtenerMaquinasPaginado(DefinicionFiltro definicionFiltro);
    }
}
