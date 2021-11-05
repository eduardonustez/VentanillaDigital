using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.InputModel
{
    public class ErrorResponse
    {
        public List<ErrorInputRequest> Errors { get; set; } = new List<ErrorInputRequest>();
    }
}
