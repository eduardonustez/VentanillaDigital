using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;

namespace Infraestructura.ContextoPrincipal.Repositorios.Transaccional
{
    public class DocumentoPrevioRepositorio : RepositorioBase<DocumentoPrevio>, IDocumentoPrevioRepositorio
    {
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        public DocumentoPrevioRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
    }
}
