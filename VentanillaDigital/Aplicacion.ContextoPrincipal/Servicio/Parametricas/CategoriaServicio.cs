using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using GenericExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class CategoriaServicio : BaseServicio, ICategoriaServicio
    {
        private const string NOMBRE_CATEGORIA_TRAMITES_DIGITALES = "Tramites Digitales";
        private ICategoriaRepositorio _categoriaRepositorio { get; }

        public CategoriaServicio(ICategoriaRepositorio categoriaRepositorio):base(categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }
        public async Task<IEnumerable<CategoriaReturnDTO>> ObtenerCategorias()
        { 
           var categorias = (await _categoriaRepositorio
                .Obtener(c => c.IsDeleted == false && 
                c.Nombre != NOMBRE_CATEGORIA_TRAMITES_DIGITALES,
                nu=>nu.TiposTramites))
                .ToList();
           return categorias.Adaptar<CategoriaReturnDTO>();
        }

        public async Task<IEnumerable<CategoriaReturnDTO>> ObtenerCategoriaNotariaVirtual()
        {
            var categorias = (await _categoriaRepositorio
                 .Obtener(c => c.IsDeleted == false &&
                 c.Nombre == NOMBRE_CATEGORIA_TRAMITES_DIGITALES,
                 nu => nu.TiposTramites))
                 .ToList();
            return categorias.Adaptar<CategoriaReturnDTO>();
        }
    }
}
