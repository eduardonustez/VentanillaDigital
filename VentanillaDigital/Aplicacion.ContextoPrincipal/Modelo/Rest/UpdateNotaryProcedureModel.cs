using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Rest
{
    public class UpdateNotaryProcedureModel
    {
        public string notaryProcedureID { get; set; }
        public int state { get; set; }
    }

    public class ResponseModel
    {
        public bool error { get; set; }
        public string data { get; set; }
        public string message { get; set; }
    }
}
