using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Nucleo
{
  public interface IUnidadDeTrabajo:IDisposable
    {
        int Commit();
        Task<int> SaveChangesAsync();
        Task<int> CommitAsync();
        void CommitAndRefreshChanges();
        Task<int> CommitAndRefreshChangesAsync();
        void RollbackChanges();
        void Refresh(object entity);
        void AplicarValoresActuales<TEntidad>(TEntidad persistida, TEntidad actual) where TEntidad : class;
        void Begin();
        void Rollback();
    }
}
