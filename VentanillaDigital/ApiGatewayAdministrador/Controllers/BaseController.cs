using ApiGateway.Models;
using ApiGatewayAdministrador.Filtro;
using Infraestructura.Transversal.HandlingError;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(ErroresDTO), 400)]
    public abstract class BaseController : Controller, IDisposable
    {
        #region Miembros
        private IList<IDisposable> _servicios;
        protected Guid TransaccionId;
        #endregion

        #region Constructor
        public BaseController(params IDisposable[] servicios)
        {
            if (servicios != null) this._servicios = servicios;
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.TransaccionId = Guid.NewGuid();
        }

        #region Miembros IDisposable
        public new void Dispose()
        {
            foreach (var servicio in this._servicios) servicio.Dispose();
            this._servicios = null;
        }
        #endregion
    }
}