using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class PersonasRepositorio : RepositorioBase<Persona>, IPersonasRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public PersonasRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion
        public async Task<Persona> Personas_Obtener(string numeroDocumento)
        {
            var tipoTramite = _unidadTrabajoContextoPrincipal.Persona.Where(x => x.NumeroDocumento == numeroDocumento).FirstOrDefault();
            return tipoTramite;
        }

        public async Task<Persona> ObtenerPorCorreo(string correo)
        {
            var tipoTramite = await _unidadTrabajoContextoPrincipal.Persona.Where(x => x.Email == correo).FirstOrDefaultAsync();
            return tipoTramite;
        }
    }
}
