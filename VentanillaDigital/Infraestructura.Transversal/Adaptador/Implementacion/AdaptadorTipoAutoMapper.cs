using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infraestructura.Transversal.Adaptador
{
    public class AdaptadorTipoAutoMapper: IAdaptadorTipo
    {
        #region Miembros

        private readonly IMapper _mapper;
        #endregion

        #region Constructor

        /// <summary>
        /// Crea una nueva instancia que adapta según el mapeador recibido
        /// </summary>
        /// <param name="mapper">mapper</param>
        public AdaptadorTipoAutoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion

        #region IAdaptadorTipo Members

        public TTarget Adaptar<TTarget>(object source) 
        {
            return _mapper.Map<TTarget>(source);
        }

        public TTarget Adaptar<TSource, TTarget>(TSource source)
        {
            return _mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adaptar<TSource, TTarget>(TSource source, TTarget target)
        {
            return _mapper.Map(source, target);
        }

        public IEnumerable<TTarget> Adaptar<TTarget>(IEnumerable<object> source)
        {
            return source.Select(s => _mapper.Map<TTarget>(s));
        }

        public IEnumerable<TTarget> Adaptar<TSource, TTarget>(IEnumerable<TSource> source)
        {
            return source.Select(s => _mapper.Map<TSource, TTarget>(s));
        }

        public IEnumerable<TTarget> Adaptar<TSource, TTarget>(IEnumerable<TSource> sources,
            IEnumerable<TTarget> targets)
        {
            var mapped = sources.Zip(targets, (source, target) => _mapper.Map(source, target));
            IEnumerable<TTarget> missing = null;
            int sCount = sources.Count();
            int tCount = targets.Count();
            if (sCount < tCount)
            {
                missing = targets.Skip(sCount);
            }
            if (tCount < sCount)
            {
                missing = sources.Skip(tCount)
                                 .Select(s => _mapper.Map<TTarget>(s));
            }
            if (missing != null)
            {
                mapped = mapped.Concat(missing);
            }
            return mapped;
        }

        public TTarget Adaptar<TTarget>(object source,
            string llaveEncriptacion,
            bool cifradoEstatico = false)
        {
            return _mapper.Map<TTarget>(source, opt =>
            {
                opt.Items[nameof(llaveEncriptacion)] = llaveEncriptacion;
                opt.Items[nameof(cifradoEstatico)] = cifradoEstatico;
            });
        }

        public TTarget Adaptar<TSource, TTarget>(TSource source, 
            string llaveEncriptacion, 
            bool cifradoEstatico = false)
        {
            return _mapper.Map<TSource, TTarget>(source, opt =>
            {
                opt.Items[nameof(llaveEncriptacion)] = llaveEncriptacion;
                opt.Items[nameof(cifradoEstatico)] = cifradoEstatico;
            });
        }

        public TTarget Adaptar<TSource, TTarget>(
            TSource source, 
            TTarget target,
            string llaveEncriptacion, 
            bool cifradoEstatico = false)
        {
            return _mapper.Map(source, target, opt =>
            {
                opt.Items[nameof(llaveEncriptacion)] = llaveEncriptacion;
                opt.Items[nameof(cifradoEstatico)] = cifradoEstatico;
            });
        }

        #endregion
    }
}
