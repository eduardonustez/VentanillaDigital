using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class PaginableResponse<T>
        where T : class
    {
        public long TotalRows { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
