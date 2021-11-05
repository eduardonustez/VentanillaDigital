using Aplicacion.ContextoPrincipal.Modelo;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Excepcion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Filtro
{
    public class AuditableFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            
            string body = "";
            try
            {
                var model = context.ActionArguments.FirstOrDefault();
                body = JsonConvert.SerializeObject(model.Value);
            }
            catch { }
            


            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();

            StringValues direccionIp = "";
            StringValues direccionMac = "";
            StringValues usuario = "";
            context.HttpContext.Request.Headers.TryGetValue("ipAddress", out direccionIp);
            context.HttpContext.Request.Headers.TryGetValue("macAddress", out direccionMac);
            context.HttpContext.Request.Headers.TryGetValue("username", out usuario);

            InformationModel informationModel = new InformationModel(""
                , PoliticaCapaEnum.ServiciosDistribuidos.ToString()
                , controllerName
                , actionName
                , "", "", body,usuario);
            IdentificacionEquipo identificacionEquipo = new IdentificacionEquipo(direccionMac, direccionIp, direccionMac);

            SerilogFactory.Create().LogInformation(informationModel,identificacionEquipo,"");

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            StringValues direccionIp = "";
            context.HttpContext.Request.Headers.TryGetValue("ipAddress", out direccionIp);

            string Peticion = string.Empty;
           
            string Metodo = context.HttpContext.Request.Method.ToString();
            string URI = context.HttpContext.Request.Host.Value.ToString();
            var request = context.HttpContext.Request;
            try
            {
                request.EnableBuffering();
                using (var stream = new StreamReader(request.Body))
                {
                    stream.BaseStream.Position = 0;
                    var requestBody = JsonConvert.DeserializeObject(stream.ReadToEnd());
                    Peticion = JsonConvert.SerializeObject(requestBody);
                }
            }
            catch { }
            
            string Respuesta = context.HttpContext.Response.StatusCode.ToString();
            var statusCode = (context.Result as ObjectResult)?.StatusCode;
            string Accion = context.HttpContext.Request.Path;
            DateTime fecha = DateTime.Now;
            //TrazabilidadAPIModel trzmod = new TrazabilidadAPIModel();
            //trzmod.TrazabilidadAPIURI = URI;
            //trzmod.TrazabilidadAPIRespuesta = Respuesta;
            //trzmod.TrazabilidadApicodigoRespuesta = context.HttpContext.Response.StatusCode;
            //trzmod.TrazabilidadAPIMetodo = Metodo;
            //if (Metodo == HttpMethod.Get.ToString())
            //    trzmod.TrazabilidadAPIPeticion = HttpMethod.Get.ToString();
            //else
            //    trzmod.TrazabilidadAPIPeticion = Peticion;
            //trzmod.TrazabilidadAPIAccion = Accion;
            //trzmod.TrazabilidadAPIFecha = fecha;
            //this.adicionarTrazaModel = trzmod;
           

        }

    }
}
