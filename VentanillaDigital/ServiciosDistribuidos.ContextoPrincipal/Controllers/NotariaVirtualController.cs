using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotariaVirtualController : ControllerBase
    {
        readonly IConvenioNotariaVirtualServicio _convenioNotariaVirtualServicio;
        public NotariaVirtualController(IConvenioNotariaVirtualServicio convenioNotariaVirtualServicio)
        {
            _convenioNotariaVirtualServicio = convenioNotariaVirtualServicio;
        }

        [HttpGet]
        [Route("ValidarConvenioNotariaVirtual")]
        public async Task<IActionResult> ValidarNotariaConvenioVirtual(ConvenioNotariaVirtualDTO convenioNotariaVirtualDTO)
        {
            var categorias = await _convenioNotariaVirtualServicio.ObtenerNotariaVirtualConvenio(convenioNotariaVirtualDTO);
            return Ok(categorias);
        }

        [HttpGet]
        [Route("ObtenerEstadosTramiteVirtual")]
        public async Task<IActionResult> ObtenerEstadosTramiteVirtual(EstadosTramiteVirtualDTO estadosTramiteVirtual) => Ok(await _convenioNotariaVirtualServicio.ObtenerEstadosTramiteVirtual(estadosTramiteVirtual));

        [HttpPost]
        [Route("ObtenerMiConfiguracionMiFirma")]
        public async Task<IActionResult> ObtenerMiConfiguracionMiFirma(ConvenioNotariaVirtualDTO convenioNotariaVirtualDTO)
        {
            var configuracionMiFirma = await _convenioNotariaVirtualServicio.ObtenerMiConfiguracionMiFirma(convenioNotariaVirtualDTO);
            return Ok(configuracionMiFirma);
        }
    }
}
