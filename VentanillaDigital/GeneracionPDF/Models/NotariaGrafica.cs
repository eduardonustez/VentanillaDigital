using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generacion_PDF_Notaria.Models
{
    public class NotariaGrafica
    {
        public class Total
        {
            public DateTime Fecha { get; set; }
            public string Dias { get; set; }
            public int HIT { get; set; }
            public int NOHIT { get; set; }
            public int TOTAL { get; set; }
        }
        public class MesTotal
        {
            public string Mes { get; set; }
            public int Total { get; set; }
        }

        public class Totales
        {
            public int HIT { get; set; }
            public int NOHIT { get; set; }
            public int TOTAL { get; set; }
        }

        public class Cotejos
        {
            public DateTime Fecha { get; set; }
            public string Dias { get; set; }
            public int MayoresA30 { get; set; }
            public int MenoresA30 { get; set; }
            public int Total { get; set; }

        }

        
    }
}
