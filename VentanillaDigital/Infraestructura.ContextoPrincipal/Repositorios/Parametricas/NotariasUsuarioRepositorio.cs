using Dominio.ContextoPrincipal.ContratoRepositorio;
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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Vista;
using Microsoft.Extensions.Caching.Memory;
using System.Transactions;
using Infraestructura.Transversal.Cache;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class NotariasUsuarioRepositorio : RepositorioBase<NotariaUsuarios>, INotariasUsuarioRepositorio
    {
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        private readonly ImplementedCache _memoryCache;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        public NotariasUsuarioRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext,
            ImplementedCache memoryCache) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
            _memoryCache = memoryCache;
        }

        public Task<PersonaDatos[]> ObtenerDatosContacto(string usuarioId, Expression<Func<PersonaDatos, bool>> selector, string include = "")
        {
            var datos = _unidadTrabajoContextoPrincipal.PersonaDatos;

            return _unidadTrabajoContextoPrincipal.NotariaUsuarios
                .Where(u => u.UsuariosId == usuarioId)
                .Join(datos, u => u.Persona.PersonaId, d => d.PersonaId, (u, d) => d)
                .Where(selector)
                .Include(include)
                .ToArrayAsync();
        }

        public Task<PersonaDatos[]> ObtenerDatosContacto(string usuarioId, string include = "")
        {
            return ObtenerDatosContacto(usuarioId, (d) => true, include);
        }


        public async Task<(DatosNotaria, string, DatosNotario, string)> ObtenerNotariaUsuarioActa(string userEmail)
        {
            var notariaUsuarioQ = base.GetSet()
                    .Where(
                       nu => nu.IsDeleted == false &&
                       nu.UserEmail == userEmail);

            var datosNotaria = await _memoryCache.GetFromCache($"DATOS_NOTARIA_{userEmail}", () => notariaUsuarioQ
                .Select(nu => new
                {
                    NotariaId = nu.NotariaId,
                    Municipio = nu.Notaria.Ubicacion.Nombre,
                    Departamento = nu.Notaria.Ubicacion.UbicacionPadre == null ? null :
                                   nu.Notaria.Ubicacion.UbicacionPadre.Nombre,
                    NumeroNotaria = nu.Notaria.NumeroNotaria,
                    NumeroNotariaEnLetras = nu.Notaria.NumeroNotariaEnLetras,
                    CirculoNotaria = nu.Notaria.CirculoNotaria,
                    FechaModificacion = nu.Notaria.FechaModificacion,

                    Nombres = nu.Persona.Nombres,
                    Apellidos = nu.Persona.Apellidos,
                    TipoNotario = nu.Notario.TipoNotario,
                    FechaModificacionNotario = nu.FechaModificacion,
                    Pin = nu.Notario.Pin,
                    Genero = nu.Persona.Genero == null ? "" : nu.Persona.Genero.ToString()
                }).FirstOrDefaultAsync()
            );

            var dgrafoNotario = await _memoryCache.GetFromCache($"Grafo:{userEmail}", () => notariaUsuarioQ
                                        .Select(nu => nu.Notario.GrafoArchivo)
                                        .FirstOrDefaultAsync());

            if (dgrafoNotario == null) throw new Exception("El notario no tiene un grafo asignado");

            var dselloNotaria = await _memoryCache.GetFromCache($"Sello:{datosNotaria.NotariaId}", () => notariaUsuarioQ
                .Select(nu => nu.Notaria.SelloArchivo)
                .FirstOrDefaultAsync());

            if (dselloNotaria == null) throw new Exception("La notaría no tiene sello asignado");

            var grafoNotario = dgrafoNotario.ToDataUrl();
            var selloNotaria = dselloNotaria.ToDataUrl();

            //var grafoNotario = await _memoryCache.GetFromCache($"Grafo:{userEmail}", () => 

            //            .Select(nu => nu.Notario.GrafoArchivo.ToDataUrl())
            //            .FirstOrDefaultAsync());

            //var selloNotaria = await _memoryCache.GetFromCache($"Sello:{datosNotaria.NotariaId}", () => notariaUsuarioQ
            //    .Select(nu => nu.Notaria.SelloArchivo.ToDataUrl())
            //    .FirstOrDefaultAsync());

            return (new DatosNotaria
            {
                NotariaId = datosNotaria.NotariaId,
                Municipio = datosNotaria.Municipio,
                Departamento = datosNotaria.Departamento,
                NumeroNotaria = datosNotaria.NumeroNotaria,
                NumeroNotariaEnLetras = datosNotaria.NumeroNotariaEnLetras,
                CirculoNotaria = datosNotaria.CirculoNotaria,
                FechaModificacion = datosNotaria.FechaModificacion
            }, selloNotaria, new DatosNotario
            {
                Nombres = datosNotaria.Nombres,
                Apellidos = datosNotaria.Apellidos,
                TipoNotario = datosNotaria.TipoNotario,
                FechaModificacion = datosNotaria.FechaModificacionNotario,
                Pin = datosNotaria.Pin,
                Genero = datosNotaria.Genero
            }, grafoNotario);
        }
    }
}
