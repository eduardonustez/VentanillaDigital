using Dominio.Nucleo.Entidad;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.StoredProcedures
{
    public class TramitesPendientes
    {
        public int Id { get; set; }
        public string TipoTramite { get; set; }
        public DateTime Fecha { get; set; }
        public int Comparecientes { get; set; }
        public string DocCompareciente { get; set; }
    }
}
