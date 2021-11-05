using System.Collections.Generic;

namespace ApiGateway.Contratos.Models
{
    public class PaginableResponse<T>
        where T : class
    {
        public long TotalRows { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
