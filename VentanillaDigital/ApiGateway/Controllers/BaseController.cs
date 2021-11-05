using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Filtro;
using ApiGateway.Models;
using Infraestructura.Transversal.HandlingError;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [ApiValidationFilterAttribute]
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
          
                if (servicios != null)
                {
                    this._servicios = servicios;
                }
            
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