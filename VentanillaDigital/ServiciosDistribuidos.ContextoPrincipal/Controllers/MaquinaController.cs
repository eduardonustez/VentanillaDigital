using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.Nucleo.Entidad;
using Infraestructura.AgenteReconoser;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Models;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class MaquinaController : BaseController
    {
        IAgenteReconoser _AgenteReconoser;
        private IMaquinaServicio _MaquinaServicio { get; }
        public MaquinaController(IMaquinaServicio MaquinaServicio, IAgenteReconoser agenteReconoser) :
            base(MaquinaServicio)
        {
            _MaquinaServicio = MaquinaServicio;
            _AgenteReconoser = agenteReconoser;
        }

        [HttpPost]
        [Route("ObtenerConfiguracionesUsuario")]
        public async Task<IActionResult> ObtenerConfiguracionesUsuario(ConfiguracionesNotariaRequestDTO configuracionesNotaria)
        {
            var result = await _MaquinaServicio.ObtenerConfiguracionesUsuario(configuracionesNotaria);
            return Ok(result);
        }

        [HttpPost]
        [Route("CrearMaquina")]
        [AuditableFilter]
        public async Task<IActionResult> CrearMaquina(MaquinaCreateDTO Maquina)
        {
            var resul = await _MaquinaServicio.CrearMaquina(Maquina);
            if (!resul.EsvalidoFinal)
            {
                string[] mensajeError = new string[1];
                mensajeError[0] = resul.Mensaje==null?"No Especificado":resul.Mensaje;
                ErrorDTO errorDTO = new ErrorDTO
                {
                    Errors = mensajeError
                };
                return BadRequest(errorDTO);
            }
            else
            {
                if (resul.MaquinaId > 0)
                {
                    InputMachineRnec registroMachineRnec = new InputMachineRnec();
                    registroMachineRnec.IdCliente = resul.IdCliente;
                    registroMachineRnec.IdConvenio = resul.IdConvenio;
                    registroMachineRnec.IdOficina = resul.IdOficina;
                    registroMachineRnec.MacEquipo = resul.MAC;
                    registroMachineRnec.IpEquipo = resul.DireccionIP;
                    registroMachineRnec.Habilitado = true;
                    var response = await _AgenteReconoser.registrarMaquinaRnec(registroMachineRnec);

                    string datos = "";
                    try
                    {
                        datos = $"Request:{JsonConvert.SerializeObject(registroMachineRnec)}, Response:{JsonConvert.SerializeObject(response)}";
                    }
                    catch { }
                    InformationModel informationModel = new InformationModel(""
                   , PoliticaCapaEnum.ServiciosDistribuidos.ToString()
                   , this.GetType().Name
                   , "Registrar Maquina RNEC"
                   , "Maquinas", "",datos, Maquina.UserName);
                    IdentificacionEquipo identificacionEquipo = new IdentificacionEquipo(Maquina.MAC, Maquina.DireccionIP, Maquina.MAC);
                    SerilogFactory.Create().LogInformation(informationModel, identificacionEquipo, "");
                }
                return Ok(resul);
            }
            
        }

        [HttpGet]
        [Route("ConsultarMaquina/{mac}")]
        public async Task<IActionResult> ConsultarMaquina(string mac)
        {
            var result = await _MaquinaServicio.ConsultarMaquina(mac);
            return Ok(result);
        }
    }
}
