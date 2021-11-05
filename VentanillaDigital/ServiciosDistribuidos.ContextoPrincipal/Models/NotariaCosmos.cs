using System.Collections.Generic;

namespace ServiciosDistribuidos.ContextoPrincipal.Models
{
    public class NotariaCosmos
    {
        public List<Map> NotariasCosmosMap
        {
            get;
            set;
        }

        public class Map
        {
            public long NotariaId
            {
                get;
                set;
            }

            public long NotariaCosmosId
            {
                get;
                set;
            }

            public string NotariaName
            {
                get;
                set;
            }
        }
    }
}