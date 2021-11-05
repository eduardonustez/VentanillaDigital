using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Models;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class TrazabilidadController : BaseController
    {
        public TrazabilidadController(ITramiteServicio tramiteServicio):
            base(tramiteServicio)
        {
            
        }

        [HttpPost]
        [Route("CrearTraza")]
        public async Task<IActionResult> CrearTraza(InformationModel model)
        {
            StringValues direccionIp = "";
            StringValues direccionMac = "";
            StringValues usuario = "";
            Request.Headers.TryGetValue("ipAddress", out direccionIp);
            Request.Headers.TryGetValue("macAddress", out direccionMac);
            Request.Headers.TryGetValue("username", out usuario);

            IdentificacionEquipo identificacionEquipo = new IdentificacionEquipo(direccionMac, direccionIp, direccionMac);

            SerilogFactory.Create().LogInformation(model, identificacionEquipo, "");
            return NoContent();
        }


    }
}
