using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas.Archivos;
using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas.Archivos
{
    public class SelloNotariaRepositorio : RepositorioBase<SelloNotaria>, ISelloNotariaRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajo;
        public IUnidadDeTrabajo UnidadTrabajo => _unidadTrabajo;
        #endregion
        #region Constructor

        public SelloNotariaRepositorio(UnidadTrabajo unidadTrabajo, IHttpContextAccessor httpContext) : base(unidadTrabajo, httpContext)
        {
            _unidadTrabajo = unidadTrabajo ?? throw new ArgumentNullException(nameof(unidadTrabajo));
        }

        #endregion
    }
}
