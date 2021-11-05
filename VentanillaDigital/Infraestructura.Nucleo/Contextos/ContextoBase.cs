using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infraestructura.Nucleo.Contextos
{
    public class ContextoBase : DbContext, IContextoUnidadDeTrabajo
    {
        private IDbContextTransaction _transaction { get; set; }
        #region Constructor
        public ContextoBase(DbContextOptions options):base(options)
        {

        }
        #endregion
        public virtual DbSet<TEntidad> CreateSet<TEntidad>()
          where TEntidad : class
        {
            return base.Set<TEntidad>();
        }
        public new virtual void Attach<TEntidad>(TEntidad item)
            where TEntidad : class
        {
            base.Entry<TEntidad>(item).State = EntityState.Unchanged;
        }
        public virtual void SetModified<TEntidad>(TEntidad item, params Expression<Func<TEntidad,object>>[] properties)
            where TEntidad : EntidadBase
        {
            var entry = base.Entry(item);
            if( properties.Length == 0)
            {
                entry.State = EntityState.Modified;
            }
            else
            {
                foreach(var prop in properties)
                {
                    entry.Property(prop).IsModified = true;
                }
                base.Entry(item).Property(x => x.UsuarioModificacion).IsModified = true;
                base.Entry(item).Property(x => x.FechaModificacion).IsModified = true;
            }
            base.Entry(item).Property(x => x.UsuarioCreacion).IsModified = false;
            base.Entry(item).Property(x => x.FechaCreacion).IsModified = false;
        }
        public virtual void ApplyCurrentValues<TEntidad>(TEntidad original, TEntidad current)
            where TEntidad : class
        {
            base.Entry<TEntidad>(original).CurrentValues.SetValues(current);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public virtual int Commit()
        {
             var resul= base.SaveChanges();
            if (_transaction!=null)
                _transaction.Commit();
            return resul;
        }
        public virtual async Task<int> CommitAsync()
        {
            var resul = await base.SaveChangesAsync();
            if (_transaction != null)
                _transaction.Commit();

            return resul;
        }
       
        public virtual void Begin()
        {
          
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            _transaction = base.Database.BeginTransaction();            
        }
        public virtual void Rollback()
        {
            _transaction.Rollback();
            foreach (var entry in base.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
            
        }
        public virtual void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    this.Commit();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

        }
        public virtual async Task<int> CommitAndRefreshChangesAsync()
        {
            bool saveFailed = false;
            int i = 0;
            do
            {
                try
                {
                    i = await this.CommitAsync();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

            return i;

        }
        public virtual void RollbackChanges()
        {
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }
        public virtual IEnumerable<TEntidad> ExecuteQuery<TEntidad>(string sqlQuery, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public virtual void Refresh(object entity)
        {
            base.Entry(entity).Reload();
        }

        /// <summary>
        /// Aplica los valores actuales a la entidad persistida <paramref name="persistida"/>
        /// </summary>
        /// <typeparam name="TEntidad">tipo de la entidad</typeparam>
        /// <param name="persistida">entidad original</param>
        /// <param name="actual">entidad actual</param>
        public void AplicarValoresActuales<TEntidad>(TEntidad persistida, TEntidad actual) where TEntidad : class
        {
            base.Entry<TEntidad>(persistida).CurrentValues.SetValues(actual);
        }

        public object IniciarTransaccion()
        {
            var trans = base.Database.BeginTransaction();
            return trans;
        }
    }
}
