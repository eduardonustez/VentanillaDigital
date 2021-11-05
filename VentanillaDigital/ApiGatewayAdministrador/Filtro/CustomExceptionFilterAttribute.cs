using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Excepcion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ApiGatewayAdministrador.Filtro
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilterAttribute(
            IModelMetadataProvider modelMetadataProvider
            )
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {

            var exception = context.Exception;
            var message = exception.Message;
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var parameters = context.ActionDescriptor.Parameters;
            string data = "";
            foreach (var parameter in parameters)
            {
                if (parameter.Name.Contains("DTO"))
                {
                    data = JsonConvert.SerializeObject(parameter.BindingInfo);
                }
            }

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
            //ErrorModel errorModelo = new ErrorModel(Guid.NewGuid().ToString(), exception, PoliticaCapaEnum.ApiGateway, controllerName,
            //    actionName, data, ""
            //      );
            //SerilogFactory.Create().LogError(errorModelo, message);
            context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(message));
        }
    }
}
