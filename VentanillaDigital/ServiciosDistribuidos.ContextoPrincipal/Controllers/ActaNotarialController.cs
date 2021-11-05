using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using AutoMapper.Configuration;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Map;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class ActaNotarialController : BaseController
    {
        private IConsultaNotariaSeguraServicio _notariaSeguraServicio { get; }
        private IActaNotarialServicio _actaNotarialServicio { get; }
        private ITramiteServicio _tramiteServicio { get; }
        readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public ActaNotarialController(IActaNotarialServicio actaNotarialServicio,
            ITramiteServicio tramiteServicio, IConsultaNotariaSeguraServicio notariaSeguraServicio,
            Microsoft.Extensions.Configuration.IConfiguration configuration) :
            base(actaNotarialServicio, actaNotarialServicio)
        {
            _notariaSeguraServicio = notariaSeguraServicio;
            _actaNotarialServicio = actaNotarialServicio;
            _tramiteServicio = tramiteServicio;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("ObtenerActaNotarial")]
        public async Task<IActionResult> ObtenerActaNotarial(ActaNotarialGetDTO actaNotarialGetDTO)
        {
            var acta = await _actaNotarialServicio.ObtenerActaNotarial(actaNotarialGetDTO);
            return Ok(acta);
        }

        [HttpGet]
        [Route("ObtenerActaNotarialPublico/{codigo}")]
        public async Task<IActionResult> ObtenerActaNotarialPublico(string codigo)
        {
            var acta = await _actaNotarialServicio.ObtenerActaNotarialPublico(codigo);
            return Ok(acta);
        }

        [HttpGet]
        [Route("ObtenerActaNotarialSegura")]
        public async Task<IActionResult> ObtenerActaNotarialSegura(ActaNotarialSeguraRequest request)
        {
            string yearFromKey = _configuration["anioLimiteConsultaHistoricos"] ?? DateTime.Now.AddYears(-1).Year.ToString();
            var anioLimite = int.Parse(yearFromKey);

            string acta;
            if (anioLimite >= request.FechaTramite.Year)
            {
                request.NotariaId = NotariaCosmosMap.GetCosmosId(request.NotariaId);
                acta = await _actaNotarialServicio.ObtenerActaNotarialSeguraHistorico(request);
            }
            else
            {
                acta = await _actaNotarialServicio.ObtenerActaNotarialSegura(request);
            }
            return Ok(acta);
        }

        [HttpPost]
        [Route("InsertConsultaNotaria")]
        public async Task<IActionResult> InsertConsultaNotaria(ConsultaNotariaSeguraInsertDTO insertConsulta)
        {
            var action = await _notariaSeguraServicio.InsertarConsultaNotariaSegura(insertConsulta);
            return Ok(action);
        }

        [HttpPost]
        [Route("CancelarTramiteNotarial")]
        public async Task<IActionResult> CancelarTramiteNotarial(TramiteRechazadoDTO pinFirma)
        {
            var resul = await _actaNotarialServicio.CancelarTramiteNotarial(pinFirma);
            return Ok(resul);
        }

        [HttpPost]
        [Route("RechazarTramiteNotarial")]
        public async Task<IActionResult> RechazarTramiteNotarial(TramiteRechazadoDTO pinFirma)
        {
            var resul = await _actaNotarialServicio.RechazarTramiteNotarial(pinFirma);
            return Ok(resul);
        }

        [HttpPost]
        [Route("FirmaActaNotarialLote")]
        public async Task<IActionResult> FirmaActaNotarialLote(AutorizacionTramitesDTO autorizacionTramites)
        {

            //var compositeTask = Task.Run(() => _actaNotarialServicio.FirmaActaNotarialLote(autorizacionTramites))
            //    .ContinueWith(resul => Trace.WriteLine(resul),
            //    TaskContinuationOptions.OnlyOnRanToCompletion);

            //_ = Task.Run(async () =>
            //{
            //    await _actaNotarialServicio.FirmaActaNotarialLote(autorizacionTramites);
            //});

            var resultadoAutorizacion = await _actaNotarialServicio.FirmaActaNotarialLote(autorizacionTramites);

            return Ok(resultadoAutorizacion);
        }
        [HttpGet]
        [Route("ObtenerResumen/{tramiteId}")]
        public async Task<IActionResult> ObtenerResumen(long tramiteId)
        {
            return Ok(await _actaNotarialServicio.ObtenerResumen(tramiteId));
        }

        [HttpPost]
        [Route("CrearActaParaFirmaManual")]
        public async Task<IActionResult> CrearActaParaFirmaManual(ActaCreateDTO actaDTO)
        {
            await _actaNotarialServicio.CrearActaParaFirmaManual(actaDTO);
            return Ok(true);
        }


    }
}
