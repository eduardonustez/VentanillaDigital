﻿using Dominio.ContextoPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class RegisteredUserDTO 
    {
        public long PersonaId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Nombre_Completo { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        
    }

  
}