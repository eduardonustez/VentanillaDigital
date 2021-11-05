using Dominio.ContextoPrincipal.Entidad.CosmosDB;
using Infraestructura.Cosmos.Interfaces;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.Cosmos.Impl
{
    public class HistoricosCosmosInfraestructura : IHistoricosCosmosInfraestructura
    {
        CosmosClient _cosmosClient { get; }
        public HistoricosCosmosInfraestructura(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<string> ObtenerRutaArchivo(ConsultaHistoricoNotariaSegura consultaHistoricoNotariaSegura)
        {
            string ruta = string.Empty;

            var peopleContainer = _cosmosClient.GetContainer("historicodb", "ucnc_uploadLog98");

            string sqlStatement = string.Format(@"SELECT * 
                                                    FROM c 
                                                    WHERE c.abstractSK = '{0}'
                                                        and c.generationTime = '{1}' 
                                                        and c.notariaCodigoCreacion = '{2}'",
                                                    consultaHistoricoNotariaSegura.Nut,
                                                    consultaHistoricoNotariaSegura.Fecha,
                                                    consultaHistoricoNotariaSegura.NotariaId);

            //var item = await peopleContainer.GetItemQueryIterator<dynamic>(sqlStatement).ReadNextAsync();
            //return item?.FirstOrDefault()?.fileName.ToString();

            var iterator = peopleContainer.GetItemQueryIterator<dynamic>(sqlStatement);
            while (iterator.HasMoreResults)
            {

                var results = iterator.ReadNextAsync().Result;

                if (results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        if (item != null)
                        {
                            if (item.fileName != null)
                            {
                                ruta = Convert.ToString(item.fileName);                               
                                break;

                            }    
                        }
                    }

                }
            }

            return ruta;
        }
    }
}