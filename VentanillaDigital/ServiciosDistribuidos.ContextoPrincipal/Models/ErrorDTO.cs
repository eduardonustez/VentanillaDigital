using Infraestructura;
using Infraestructura.Transversal;
using Infraestructura.Transversal.Log;
using Infraestructura.Transversal.Log.Modelo;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiciosDistribuidos.ContextoPrincipal.Models
{
    public class ErrorDTO
    {
        public string[] Errors { get; set; }
    }
}
