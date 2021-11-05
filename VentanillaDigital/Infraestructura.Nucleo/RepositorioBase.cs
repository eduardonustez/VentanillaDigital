using Dominio.Nucleo;
using Infraestructura.Nucleo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dominio.Nucleo.Entidad;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Infraestructura.Repositorios
{
    public class RepositorioBase<TEntidad> : IRepositorioBase<TEntidad> 
        where TEntidad : EntidadBase
    {
       
       protected readonly IContextoUnidadDeTrabajo _contextoUnidadDeTrabajo;
       protected readonly IHttpContextAccessor _httpContext;


        #region Constructor
        public RepositorioBase(IContextoUnidadDeTrabajo contextoUnidadDeTrabajo, IHttpContextAccessor httpContext)
        {
            if (contextoUnidadDeTrabajo == (IUnidadDeTrabajo)null)
                throw new ArgumentNullException(
                    nameof(IUnidadDeTrabajo));

            _contextoUnidadDeTrabajo = contextoUnidadDeTrabajo;
            _httpContext = httpContext;

        }
        
        public RepositorioBase(IContextoUnidadDeTrabajo contextoUnidadDeTrabajo)
        {
            if (contextoUnidadDeTrabajo == (IUnidadDeTrabajo)null)
                throw new ArgumentNullException(
                    nameof(IUnidadDeTrabajo));

            _contextoUnidadDeTrabajo = contextoUnidadDeTrabajo;

        }
        #endregion

        #region MetodosPrivados
        protected DbSet<TEntidad> GetSet()
        {
            return _contextoUnidadDeTrabajo.CreateSet<TEntidad>();
        }
        #endregion
        #region Metodos IRepositorioBase
        public IUnidadDeTrabajo UnidadDeTrabajo
        {
            get
            {
                return _contextoUnidadDeTrabajo;
            }
        }
        public virtual TEntidad Obtener(object id)
        {
            try
            {
                if (id != null)
                    return GetSet().Find(id);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual async Task<TEntidad> ObtenerAsync(object id)
        {
            if (id != null)
            {
                return await GetSet().FindAsync(id);
            }
            else
                return null;
        }

        public virtual IQueryable<TEntidad> ObtenerTodo(bool includeDeleted = false)
        {
            IQueryable<TEntidad> set = GetSet();
            if (!includeDeleted)
                set = set.Where(t => !t.IsDeleted);
            return set;
        }

        public virtual async Task<IEnumerable<TEntidad>> ObtenerTodoAsync(bool includeDeleted, params Expression<Func<TEntidad,object>>[] includes)
        {
            IQueryable<TEntidad> ret = GetSet();
            if (!includeDeleted)
                ret = ret.Where(t => !t.IsDeleted);
            foreach (var inc in includes)
                ret = ret.Include(inc);
            return await ret.ToListAsync();
        }
        public virtual Task<IEnumerable<TEntidad>> ObtenerTodoAsync(params Expression<Func<TEntidad, object>>[] includes)
        {
            return ObtenerTodoAsync(false, includes);
        }

        public Task<TEntidad> GetOneAsync(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes)
        {
            var set = GetSet().Where(predicate);
            if (!includeDeleted)
                set = set.Where(t => !t.IsDeleted);
            foreach (var item in includes)
                set = set.Include(item);
            return set.FirstOrDefaultAsync();
        }

        public Task<TEntidad> GetOneAsync(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes)
        {
            return GetOneAsync(predicate, false, includes);
        }

        public virtual async Task<IEnumerable<TEntidad>> Obtener(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes)
        {
            var set = GetSet().Where(predicate);
            if (!includeDeleted)
                set = set.Where(t => !t.IsDeleted);
            foreach (var item in includes)
            {
                set = set.Include(item);
            }
            return await set.ToListAsync();
        }

        public virtual Task<IEnumerable<TEntidad>> Obtener(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes)
        {
            return Obtener(predicate, false, includes);
        }

        public virtual IEnumerable<TEntidad> ObtenerSync(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes)
        {
            var set = GetSet().Where(predicate);
            if (!includeDeleted)
                set = set.Where(t => !t.IsDeleted);
            foreach (var item in includes)
            {
                set = set.Include(item);
            }
            return set.ToList();
        }

        public virtual IEnumerable<TEntidad> ObtenerSync(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes)
        {
            return ObtenerSync(predicate, false, includes);
        }

        public virtual async Task<IEnumerable<TEntidad>> ObtenerPaginado(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted
            , int pageSize = 10, int pageNumber = 1, params Expression<Func<TEntidad, object>>[] includes)
        {
           
            var query = GetSet()
                .Where(predicate);
            if(!includeDeleted)
                query=query.Where(q => !q.IsDeleted);
            
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            var totalPages = Math.Ceiling(query.Count() / (double)pageSize);
            _httpContext.HttpContext.Response.Headers.Add("x-current-page", pageNumber.ToString());
            _httpContext.HttpContext.Response.Headers.Add("x-items-per-page", pageSize.ToString());
            _httpContext.HttpContext.Response.Headers.Add("x-total-items", query.Count().ToString());
            _httpContext.HttpContext.Response.Headers.Add("x-total-pages", totalPages.ToString());
            _httpContext.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", new string[] { "x-current-page", "x-items-per-page",
                "x-total-items", "x-total-pages"});

            return await query
                .OrderByDescending(e => e.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public virtual Task<IEnumerable<TEntidad>> ObtenerPaginado(Expression<Func<TEntidad, bool>> predicate
            , int pageSize = 10, int pageNumber = 1, params Expression<Func<TEntidad, object>>[] includes)
        {
            return ObtenerPaginado(predicate, false, pageSize, pageNumber,includes);
        }

        public TEntidad Agregar(TEntidad item)
        {
            if (item != (TEntidad)null)
            {
                item.FechaCreacion = DateTime.Now;
                item.FechaModificacion = DateTime.Now;
                return GetSet().Add(item).Entity;
            }
            else
            {
                throw new Exception(nameof(item));
            }
        }

        public void Agregar(IEnumerable<TEntidad> items)
        {
            if (items != null)
            {
                foreach (var item in items) this.Agregar(item);
            }
            else
            {
                throw new Exception(nameof(items));
            }
        }


        //public void Eliminar(TEntidad item)
        //{
        //    if (item != (TEntidad)null)
        //    {
        //        _contextoUnidadDeTrabajo.Attach(item);
        //        GetSet().Remove(item);
        //    }
        //    else
        //    {
        //        throw new Exception(nameof(item));
        //    }
        //}

        //public void Eliminar(IEnumerable<TEntidad> items)
        //{
        //    if (items != null)
        //    {
        //        foreach (var item in items) this.Eliminar(item);
        //    }
        //    else
        //    {
        //        throw new Exception(nameof(items));
        //    }
        //}
        private void EliminarLogico(TEntidad item)
        {
            if (item != (TEntidad)null)
            {
                 item.IsDeleted = true;
                item.FechaModificacion = DateTime.Now;
                _contextoUnidadDeTrabajo.SetModified(item);
            }
               
            else
            {
                throw new Exception(nameof(item));
            }
        }
        private void EliminarFisico(TEntidad item)
        {
            if (item != (TEntidad)null)
            {
                _contextoUnidadDeTrabajo.Attach(item);
                GetSet().Remove(item);
            }
            else
            {
                throw new Exception(nameof(item));
            }
        }
        public void Eliminar(TEntidad item, bool fisico = false)
        {
            if (fisico)
                EliminarFisico(item);
            else
                EliminarLogico(item);
        }
        public void Eliminar(IEnumerable<TEntidad> items)
        {
            if (items != null)
            {
                foreach (var item in items) this.Eliminar(item);
            }
            else
            {
                throw new Exception(nameof(items));
            }
        }

        public void Modificar(TEntidad item, params Expression<Func<TEntidad,object>>[] propiedades)
        {
            if (item != (TEntidad)null)
            {
                item.FechaModificacion = DateTime.Now;
                _contextoUnidadDeTrabajo.SetModified(item, propiedades);
            }
           
            else
            {
                throw new Exception(nameof(item));
            }
        }

        public void Modificar(IEnumerable<TEntidad> items)
        {
            if (items != null)
            {
                foreach (var item in items) this.Modificar(item);
            }
            else
            {
                throw new Exception(nameof(items));
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (_contextoUnidadDeTrabajo != null)
                _contextoUnidadDeTrabajo.Dispose();
        }

        #endregion
    }
}
