﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class TipoIdentificacionModel
    {
        public int TipoIdentificacionId { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
    }
}
