using Aplicacion.ContextoPrincipal.Modelo;
using Infraestructura.Transversal.Excepciones;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Excepcion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ServiciosDistribuidos.ContextoPrincipal.Filtro
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        
        public CustomExceptionFilterAttribute(
            IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }
      
        public override void OnException(ExceptionContext context)
        {

            var exception = context.Exception;
            var message = exception.Message;

            if (exception is ApplicationValidationErrorsException)
            {
                var validationErrors = ((ApplicationValidationErrorsException)exception).ValidationErrors;

                if (validationErrors != null)
                {
                    foreach (var error in validationErrors)
                    {
                        message += Environment.NewLine + error;
                    }
                }
            }
            
            if(exception is DbUpdateException && exception.InnerException != null && exception.InnerException is SqlException)
            {
                message = String.Empty;
                switch (exception.InnerException.Message)
                {
                    case string a when a.Contains("IX_Unique_Participante_Email"):
                        message += "El email ya se encuentra registrado";
                        break;
                    case string a when a.Contains("FK_AspNetUserTokens_AspNetUsers_UserId"):
                        message += "El email ya se encuentra registrado";
                        break;
                    case string a when a.Contains("IX_Inique_participante_Identificacion"):
                        message += "El documento ya se encuentra registrado";
                        break;
                    case string a when a.Contains("conflicted with the FOREIGN KEY constraint"):
                        int startPos = a.LastIndexOf("column ") + "column ".Length + 1;
                        int length = a.IndexOf(".\r\nThe statement has been terminated") - startPos;
                        string sub = a.Substring(startPos, length);
                        message += string.Format("El valor del campo {0} no existe",sub);
                        break;
                    default:
                        message += exception.InnerException.Message;
                        break;
                }
            }
            
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var parameters = context.ActionDescriptor.Parameters;
            string data = "";
            RequestDTO request= new RequestDTO();
            foreach (var parameter in parameters)
            {
                if (parameter.Name.Contains("DTO"))
                {
                    data = JsonConvert.SerializeObject(parameter.BindingInfo);
                    request = JsonConvert.DeserializeObject<RequestDTO>(data);
                }
            }
            if (request.transaccionGuid == null)
            {
                request.transaccionGuid = Guid.NewGuid().ToString();
            }

            StringValues direccionIp = "";
            StringValues direccionMac = "";
            StringValues usuario = "";
            context.HttpContext.Request.Headers.TryGetValue("ipAddress", out direccionIp);
            context.HttpContext.Request.Headers.TryGetValue("macAddress", out direccionMac);
            context.HttpContext.Request.Headers.TryGetValue("username", out usuario);

            ErrorModel errorModelo = new ErrorModel(request.transaccionGuid, exception, PoliticaCapaEnum.ServiciosDistribuidos, controllerName,
                actionName, data,usuario
                  );
            IdentificacionEquipo identificacionEquipo = new IdentificacionEquipo(direccionMac,direccionIp,direccionMac);
            SerilogFactory.Create().LogError(errorModelo,identificacionEquipo, $"{message}\n{exception.ToString()}");

            if (exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(new ApiBadRequestResponse(message));
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(message));
            }
            
        }
    }
}
