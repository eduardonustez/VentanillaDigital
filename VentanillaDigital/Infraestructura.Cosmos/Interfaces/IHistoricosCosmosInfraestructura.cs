using Dominio.ContextoPrincipal.Entidad.CosmosDB;
using System.Threading.Tasks;

namespace Infraestructura.Cosmos.Interfaces
{
    public interface IHistoricosCosmosInfraestructura
    {
        Task<string> ObtenerRutaArchivo(ConsultaHistoricoNotariaSegura consultaHistoricoNotariaSegura);
    }
}