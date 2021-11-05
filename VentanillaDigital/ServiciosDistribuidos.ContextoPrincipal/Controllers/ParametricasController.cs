using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class ParametricasController : BaseController
    {
        private ICategoriaServicio _categoriaServicio { get; }
        private ITipoIdentificacionServicio _tipoIdentificacionServicio { get; }
        private IUbicacionServicio _ubicacionServicio { get; }
        public ParametricasController(ICategoriaServicio categoriaServicio
            ,ITipoIdentificacionServicio tipoIdentificacionServicio
            ,IUbicacionServicio ubicacionServicio):
            base(categoriaServicio,tipoIdentificacionServicio,ubicacionServicio)
        {
            _categoriaServicio = categoriaServicio;
            _tipoIdentificacionServicio = tipoIdentificacionServicio;
            _ubicacionServicio = ubicacionServicio;
        }

            
        [HttpGet]
        [Route("ObtenerCategorias")]
        public async Task<IActionResult> ObtenerCategorias()
        {
            var categorias= await _categoriaServicio.ObtenerCategorias();
            return Ok(categorias);
        }

        [HttpGet]
        [Route("ObtenerCategoriaNotariaVirtual")]
        public async Task<IActionResult> ObtenerCategoriaNotariaVirtual()
        {
            var categorias = await _categoriaServicio.ObtenerCategoriaNotariaVirtual();
            return Ok(categorias);
        }

        [HttpGet]
        [Route("ObtenerTiposIdentificacion")]
        public async Task<IActionResult> ObtenerTiposIdentificacion()
        {
            var tiposIdentificacion= await _tipoIdentificacionServicio.ObtenerTiposIdentificacion();
            return Ok(tiposIdentificacion);
        }
        [HttpGet]
        [Route("ObtenerDepartamentos")]
        public async Task<IActionResult> ObtenerDepartamentos()
        {
            var resul = await _ubicacionServicio.GetDepartamentos();
            return Ok(resul);
        }


    }
}
