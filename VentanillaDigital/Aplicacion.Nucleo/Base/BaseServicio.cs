using Aplicacion.Nucleo.Observador;
using AutoMapper;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aplicacion.Nucleo.Base
{
    public abstract class BaseServicio:Observable,IDisposable
    {

        #region Miembros
    
        private IList<IDisposable> _servicios;
      
        #endregion
       
        public BaseServicio(params IDisposable[] servicios)
        {
            
            if (servicios != null)
            {
                this._servicios = servicios;
            }
           
        }
        public string GetCurrentMethodName()
        {
            //StackTrace stackTrace = new StackTrace();
            //StackFrame stackFrame = stackTrace.GetFrame(1);

            //return stackFrame.GetMethod().Name;
            return new StackTrace(1).GetFrame(0).GetMethod().Name;
        }
               
     

        #region Miembros IDisposable

        public void Dispose()
        {
            if (_servicios != null)
            {
                foreach (var servicio in this._servicios) servicio.Dispose();
                this._servicios = null;
            }
        }

        #endregion

    }
}
