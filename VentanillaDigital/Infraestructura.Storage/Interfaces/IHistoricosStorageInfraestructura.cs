
using System.Threading.Tasks;

namespace Infraestructura.Storage.Interfaces
{
    public interface IHistoricosStorageInfraestructura
    {
        Task<string> ObtenerActa(string rutaArchivo);
    }
}