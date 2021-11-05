using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Nucleo
{
    public interface IRepositorioBase<TEntidad> : IDisposable
         where TEntidad : EntidadBase
    {
        IUnidadDeTrabajo UnidadDeTrabajo { get; }
        TEntidad Obtener(object id);
        Task<IEnumerable<TEntidad>> Obtener(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes);
        Task<IEnumerable<TEntidad>> Obtener(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes);
        IEnumerable<TEntidad> ObtenerSync(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes);
        IEnumerable<TEntidad> ObtenerSync(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes);
        Task<TEntidad> GetOneAsync(Expression<Func<TEntidad, bool>> predicate, params Expression<Func<TEntidad, object>>[] includes);
        Task<TEntidad> GetOneAsync(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes);
        Task<TEntidad> ObtenerAsync(object id);
        IQueryable<TEntidad> ObtenerTodo(bool includeDeleted = false);
        Task<IEnumerable<TEntidad>> ObtenerTodoAsync(params Expression<Func<TEntidad, object>>[] includes);
        Task<IEnumerable<TEntidad>> ObtenerTodoAsync(bool includeDeleted, params Expression<Func<TEntidad, object>>[] includes);
        Task<IEnumerable<TEntidad>> ObtenerPaginado(Expression<Func<TEntidad, bool>> predicate, int pageSize = 10, int pageNumber = 1, params Expression<Func<TEntidad, object>>[] includes);
        Task<IEnumerable<TEntidad>> ObtenerPaginado(Expression<Func<TEntidad, bool>> predicate, bool includeDeleted, int pageSize = 10, int pageNumber = 1, params Expression<Func<TEntidad, object>>[] includes);
        TEntidad Agregar(TEntidad item);
        void Agregar(IEnumerable<TEntidad> items);
        void Eliminar(TEntidad item, bool fisico = false);
        void Eliminar(IEnumerable<TEntidad> items);
        void Modificar(TEntidad item, params Expression<Func<TEntidad, object>>[] propiedades);
        void Modificar(IEnumerable<TEntidad> items);

        #region ComportamientosPorDefinir
        ///// <summary>
        /////Track entity into this repository, really in UnitOfWork. 
        /////In EF this can be done with Attach and with Update in NH
        ///// </summary>
        ///// <param name="item">Item to attach</param>
        //void TrackItem(TEntidad item);

        ///// <summary>
        /////Track entity into this repository, really in UnitOfWork. 
        /////In EF this can be done with Attach and with Update in NH
        ///// </summary>
        ///// <param name="items">Items to attach</param>
        //void TrackItem(IEnumerable<TEntidad> item);

        ///// <summary>
        ///// Sets modified entity into the repository. 
        ///// When calling Commit() method in UnitOfWork 
        ///// these changes will be saved into the storage
        ///// </summary>
        ///// <param name="persisted">The persisted item</param>
        ///// <param name="current">The current item</param>
        //void Merge(TEntidad persisted, TEntidad current);

        ///// <summary>
        ///// Get all elements of type TEntidad that matching a
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <returns></returns>
        //IEnumerable<TEntidad> AllMatching(ISpecification<TEntidad> specification);

        ///// <summary>
        ///// Get all elements of type TEntidad that matching a - Async
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <returns></returns>
        //Task<IEnumerable<TEntidad>> AllMatchingAsync(ISpecification<TEntidad> specification);

        ///// <summary>
        ///// Get the first element of type TEntidad that matching a
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <returns></returns>
        //TEntidad FirstMatching(ISpecification<TEntidad> specification);

        ///// <summary>
        ///// Get the first element of type TEntidad that matching a - Async
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <returns></returns>
        //Task<TEntidad> FirstMatchingAsync(ISpecification<TEntidad> specification);

        ///// <summary>
        ///// Get the first element of type TEntidad that matching a
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <param name="orderByExpression">Order by expression</param>
        ///// <param name="ascending">Ascending</param>
        ///// <returns></returns>
        //TEntidad FirstMatching<KProperty>(ISpecification<TEntidad> specification, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get the first element of type TEntidad that matching a
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <param name="orderByExpression">Order by expression</param>
        ///// <param name="ascending">Ascending</param>
        ///// <returns></returns>
        //Task<TEntidad> FirstMatchingAsync<KProperty>(ISpecification<TEntidad> specification, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get all elements of type TEntidad that matching a 
        ///// Specification <paramref name="specification"/> with paged paramenters
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //IEnumerable<TEntidad> AllMatching<KProperty>(ISpecification<TEntidad> specification, int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get all elements of type TEntidad that matching a - Async
        ///// Specification <paramref name="specification"/> with paged paramenters
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //Task<IEnumerable<TEntidad>> AllMatchingAsync<KProperty>(ISpecification<TEntidad> specification, int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get all elements of type TEntidad in repository
        ///// </summary>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //IEnumerable<TEntidad> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get all elements of type TEntidad in repository - Async
        ///// </summary>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //Task<IEnumerable<TEntidad>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get  elements of type TEntidad in repository
        ///// </summary>
        ///// <param name="filter">Filter that each element do match</param>
        ///// <returns>List of selected elements</returns>
        //IEnumerable<TEntidad> GetFiltered(Expression<Func<TEntidad, bool>> filter);

        ///// <summary>
        ///// Get  elements of type TEntidad in repository - Async
        ///// </summary>
        ///// <param name="filter">Filter that each element do match</param>
        ///// <returns>List of selected elements</returns>
        //Task<IEnumerable<TEntidad>> GetFilteredAsync(Expression<Func<TEntidad, bool>> filter);

        ///// <summary>
        ///// Get  elements of type TEntidad in repository
        ///// </summary>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //IEnumerable<TEntidad> GetFiltered<KProperty>(Expression<Func<TEntidad, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Get  elements of type TEntidad in repository - Async
        ///// </summary>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageCount">Number of elements in each page</param>
        ///// <param name="orderByExpression">Order by expression for this query</param>
        ///// <param name="ascending">Specify if order is ascending</param>
        ///// <returns>List of selected elements</returns>
        //Task<IEnumerable<TEntidad>> GetFilteredAsync<KProperty>(Expression<Func<TEntidad, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntidad, KProperty>> orderByExpression, bool ascending);

        ///// <summary>
        ///// Refresh entity. Note. This generates adhoc queries.
        ///// </summary>
        ///// <param name="entity"></param>
        ////void Refresh(TEntidad entity);
        #endregion
    }
}
