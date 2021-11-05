using Aplicacion.ContextoPrincipal.Contrato;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : BaseController
    {
        //
        private readonly INotariasUsuarioServicio _notariasUsuarioServicio;

        public FuncionarioController(INotariasUsuarioServicio notariasUsuarioServicio)
        {
            _notariasUsuarioServicio = notariasUsuarioServicio;
        }

        [HttpGet]
        [Route("Contacto/{usuarioId}")]
        public async Task<IActionResult> ObtenerContacto(string usuarioId)
        {
            var datos = await _notariasUsuarioServicio.ObtenerDatosDeContacto(usuarioId);
            return Ok(datos);
        }

    }
}
