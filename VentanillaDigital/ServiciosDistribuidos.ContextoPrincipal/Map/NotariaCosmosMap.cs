using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServiciosDistribuidos.ContextoPrincipal.Map
{
    public static class NotariaCosmosMap
    {
        static NotariaCosmosMap()
        {
            if(NotariasCosmos == null)
            {
                string path = "/notariascosmosmap.json";
                var notariasCosmosMapPath = ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);
                NotariasCosmos = JsonConvert.DeserializeObject<NotariaCosmos>(notariasCosmosMapPath).NotariasCosmosMap;
            }
        }

        public static long GetCosmosId(long notariaId)
        {
            return NotariasCosmos.FirstOrDefault(x => x.NotariaId == notariaId).NotariaCosmosId;
        }

        internal static Func<string, string> ReadAllText
        {
            get 
            {
                if (readAllTextFunc == null)
                    readAllTextFunc = File.ReadAllText;
                return readAllTextFunc;
            }
            set 
            {
                readAllTextFunc = value;
            }
        }

        private static Func<string, string> readAllTextFunc;
        private static readonly List<NotariaCosmos.Map> NotariasCosmos;
    }
}