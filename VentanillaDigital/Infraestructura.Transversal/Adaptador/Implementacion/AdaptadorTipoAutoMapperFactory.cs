using AutoMapper;
using Infraestructura.Transversal.Log.Enumeracion;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infraestructura.Transversal.Adaptador
{
    public class AdaptadorTipoAutoMapperFactory : IAdaptadorTipoFactory
    {
        #region Miembros

        private IMapper _mapper = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AdaptadorTipoAutoMapperFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion

        #region IAdaptadorTipoFactory Members

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public IAdaptadorTipo Create() 
        {
            return new AdaptadorTipoAutoMapper(_mapper);
        }

        #endregion
    }
}
