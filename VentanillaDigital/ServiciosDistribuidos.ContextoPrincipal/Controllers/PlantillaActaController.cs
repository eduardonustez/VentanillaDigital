using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.Nucleo.Entidad;
using DotNetCore.Extensions;
using HtmlAgilityPack;
using Infraestructura.Transversal;
using Infraestructura.Transversal.ExtensionMethods;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class PlantillaActaController : BaseController
    {
        private IPlantillaActaServicio _plantillaActaServicio { get; }
        public PlantillaActaController(IPlantillaActaServicio plantillaActaServicio):
            base(plantillaActaServicio)
        {
            _plantillaActaServicio = plantillaActaServicio;            
        }

            
        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear(IFormFile file)
        {

            string contenido = file.GetHtmlStringFromFormFile();
            
            if (string.IsNullOrWhiteSpace(contenido))
            {
                throw new ArgumentException("Archivo no válido");
            }
            var plantilla = new PlantillaActaCreateDTO()
            {
                Contenido = contenido,
                Nombre = file.FileName
            };
            await _plantillaActaServicio.Crear(plantilla);
            return NoContent();
        }
        [HttpPost]
        [Route("Actualizar/{plantillaId}")]
        public async Task<IActionResult> Actualizar(long plantillaId, IFormFile file)
        {
            string contenido = file.GetHtmlStringFromFormFile();
            var plantilla = new PlantillaActaEditDTO()
            {
                PlantillaActaId = plantillaId,
                Contenido = contenido,
                Nombre = file.FileName
            };
            await _plantillaActaServicio.Actualizar(plantilla);
            return NoContent();
        }
        [HttpPost]
        [Route("AsociarTiposTramites")]
        public async Task<IActionResult> AsociarTiposTramites(PlantillaActaAssociateDTO plantillaActaAssociateDTO)
        {
            await _plantillaActaServicio.AsociarTiposTramites(plantillaActaAssociateDTO);
            return NoContent();
        }

        [HttpGet]
        [Route("ObtenerPlantillas")]
        public async Task<IActionResult> ObtenerPlantillas()
        {
            return Ok(await _plantillaActaServicio.ObtenerPlantillas());
        }
        [HttpGet]
        [Route("ObtenerPlantillas/{Id}")]
        public async Task<IActionResult> ObtenerPlantillaPorId(long Id)
        {
            return Ok(await _plantillaActaServicio.ObtenerPlantillaPorId(Id));
        }

        //private string GetHtmlStringFromFormFile(IFormFile file)
        //{
        //    HtmlDocument htmlDocument = new HtmlDocument();
        //    if (file.Length > 0)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            file.CopyTo(ms);
        //            var fileBytes = ms.ToArray();
        //            using (MemoryStream htmlStream = new MemoryStream(fileBytes))
        //            {
        //                htmlDocument.Load(htmlStream);
        //            }

        //        }
        //    }
        //    return htmlDocument.Text;
        //}
    }
}
