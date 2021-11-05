using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class ConsultaNotariaRepositorio : RepositorioBase<ConsultaNotariaSegura>, IConsultaNotariaSeguraRepositorio
    {
        private IConsultaNotariaSeguraRepositorio _consultaNotariaSeguraRepositorio;
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        private IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        public ConsultaNotariaRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContextAccessor): base(unidadTrabajoContextoPrincipal,httpContextAccessor)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal;
        }

        
       
    }
}
