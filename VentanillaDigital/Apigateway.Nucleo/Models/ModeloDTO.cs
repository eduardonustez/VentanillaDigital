using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Text.Json.Serialization;

namespace ApiGateway.Models
{
    public abstract class ModeloDTO
    {
        [Required]
        public Guid TransaccionGuid { get; set; }
        
    }
    public abstract class NewRegisterDTO : ModeloDTO
    {
        
        public string UserId { get; set; }
                
        public string UserName { get; set; }
        public long NotariaId { get; set; }

    }
    public abstract class EditRegisterDTO : ModeloDTO
    {
        [Required]
        
        public string UserId { get; set; }
        [Required]
       
        public string UserName { get; set; }

    }

    public class RequestDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string transaccionGuid { get; set; }
      
    }
}
