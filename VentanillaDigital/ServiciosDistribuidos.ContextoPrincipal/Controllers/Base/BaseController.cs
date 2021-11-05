using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad;
using Infraestructura.Transversal.HandlingError;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [ApiValidationFilterAttribute]
   
    public abstract class BaseController : Controller, IDisposable
    {
        #region Miembros
        private IList<IDisposable> _servicios;
      
        private IDictionary<string,object> parametros;
        

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