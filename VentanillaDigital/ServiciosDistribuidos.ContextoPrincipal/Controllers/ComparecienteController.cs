using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Microsoft.AspNetCore.Mvc;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class ComparecienteController : BaseController
    {
        private IComparecienteServicio _comparecienteServicio { get; }
        public ComparecienteController(IComparecienteServicio comparecienteServicio) : base (comparecienteServicio)
        {
            _comparecienteServicio = comparecienteServicio;
        }

        [HttpPost]
        [Route("CrearCompareciente")]
        [AuditableFilter]
        public async Task<IActionResult> CrearCompareciente(ComparecienteCreateDTO compareciente)
        {
            ComparecienteCreateReturnDTO resultado = await _comparecienteServicio.AgregarCompareciente(compareciente);
            return Ok(resultado);
        }
        

        [HttpGet]
        [Route("ObtenerPorTramiteId/{tramiteId}")]
        public async Task<IActionResult> ObtenerPorTramiteId(long tramiteId)
        {
            var tramite = await _comparecienteServicio.ObtenerComparecientesPorTramiteID(tramiteId);
            return Ok(tramite);
        }
    }
}
