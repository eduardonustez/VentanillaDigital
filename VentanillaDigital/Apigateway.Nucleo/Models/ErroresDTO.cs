using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class ErroresDTO
    {
        public string[] Errors { get; set; }
    }


    public class InputErrorDTO
    {
        public ICollection<InputErroresDTO> errors { get; set; }
    }

    public class InputErroresDTO
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
