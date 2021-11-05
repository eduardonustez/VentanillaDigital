using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class TemplateRepositorio : RepositorioBase<Parametrica>, ITemplateRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public TemplateRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion

        #region Contratos
        public InformacionUsuarioPlantilla ObtenerInformacionUsuario(string Email)
        {
            InformacionUsuarioPlantilla info = new InformacionUsuarioPlantilla();
            var query = from NotariaUsuario in _unidadTrabajoContextoPrincipal.NotariaUsuarios
                        join Notaria in _unidadTrabajoContextoPrincipal.Notaria
                            on NotariaUsuario.NotariaId equals Notaria.NotariaId
                        join Persona in _unidadTrabajoContextoPrincipal.Persona
                            on NotariaUsuario.PersonaId equals Persona.PersonaId
                        where (NotariaUsuario.UserEmail == Email)
                        select new { NombreNotaria = Notaria.Nombre, NombreUsuario = Persona.Nombres };

            bool esValido = query.Any();
            if (esValido)
            {
                info.NombreNotaria = query.FirstOrDefault().NombreNotaria;
                info.Usuario = query.FirstOrDefault().NombreUsuario;
            }

            return info;
        }

        public string ObtenerTemplate(string Codigo)
        {
            string resultado = string.Empty;
            var esParametro = _unidadTrabajoContextoPrincipal.Parametrica.Where(x => x.Codigo == Codigo).Any();
            if (esParametro)
            {
                string valor = _unidadTrabajoContextoPrincipal.Parametrica.Where(x => x.Codigo == Codigo).FirstOrDefault().Valor;

                bool esEntero = int.TryParse(valor, out int Id);
                if (esEntero)
                {
                    var esPlantillaExistente = _unidadTrabajoContextoPrincipal.PlantillaActa.Where(x => x.PlantillaActaId == Id).Any();
                    if (esPlantillaExistente)
                    {
                        var template = _unidadTrabajoContextoPrincipal.PlantillaActa.Where(x => x.PlantillaActaId == Id).FirstOrDefault().Contenido;
                        resultado = template;
                    }
                }
            }

            return resultado;
        }
        #endregion
    }
}
