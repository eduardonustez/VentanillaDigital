using Infraestructura.Transversal.Adaptador;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GenericExtensions
{
    public static class AdaptadorMetodoExtension
    {
        public static TTarget Adaptar<TSource, TTarget>(this TSource source) 
        {
            return AdaptadorTipoFactory.Create().Adaptar<TSource, TTarget>(source);
        }
        public static TTarget Adaptar<TTarget>(this object source) 
        {
            return AdaptadorTipoFactory.Create().Adaptar<TTarget>(source);
        }
        public static IEnumerable<TTarget> Adaptar<TTarget>(this IEnumerable<object> source)
        {
            return AdaptadorTipoFactory.Create().Adaptar<TTarget>(source);
        }
        public static TTarget Anadir<TTarget>(this TTarget target, object source) 
        {
            return AdaptadorTipoFactory.Create().Adaptar(source,target);
        }
    }
}
