using ApiGateway.Contratos.Models.Archivos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models.Transaccional
{
    public class ExcepcionHuellaModel
    {
        public string Descripcion { get; set; }
        public EnumDedos[] Dedos { get; set; }
    }
}
