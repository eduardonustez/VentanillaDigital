using System.Collections.Generic;

namespace Infraestructura.Transversal.Adaptador
{
    public interface IAdaptadorTipo
    {
        #region Métodos
        TTarget Adaptar<TTarget>(object source);
        TTarget Adaptar<TSource, TTarget>(TSource source);
        TTarget Adaptar<TSource, TTarget>(TSource source, TTarget target);
        IEnumerable<TTarget> Adaptar<TTarget>(IEnumerable<object> sources);
        IEnumerable<TTarget> Adaptar<TSource, TTarget>(IEnumerable<TSource> sources);
        IEnumerable<TTarget> Adaptar<TSource, TTarget>(IEnumerable<TSource> sources,
            IEnumerable<TTarget> targets);
        TTarget Adaptar<TTarget>(object source, string llaveEncriptacion, 
            bool cifradoEstatico = false);
        TTarget Adaptar<TSource, TTarget>(TSource source, string llaveEncriptacion,
            bool cifradoEstatico = false);
        TTarget Adaptar<TSource, TTarget>(TSource source, TTarget target, string llaveEncriptacion,
            bool cifradoEstatico = false);
        #endregion
    }
}
