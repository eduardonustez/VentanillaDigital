using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Text.Json.Serialization;

namespace ApiGateway.Models
{ 

    public class ResponseDTO
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public bool EsValido { get; set; }
    }
}
