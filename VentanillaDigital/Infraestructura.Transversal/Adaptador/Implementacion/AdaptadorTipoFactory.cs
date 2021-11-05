using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Adaptador
{
    public class AdaptadorTipoFactory
    {
        #region Miembros
        static IAdaptadorTipoFactory _currentAdaptadorTipoFactory = null;
        static IAdaptadorTipo _adaptadorTipo = null;
        #endregion

        #region Métodos Estáticos
        public static void SetCurrent(IAdaptadorTipoFactory adapterFactory)
        {
            _currentAdaptadorTipoFactory = adapterFactory;
        }
        public static IAdaptadorTipo Create()
        {
            if (_currentAdaptadorTipoFactory == null)
                throw new ApplicationException(Mensaje.Recursos.Excepcion_TipoAdaptador);

            if (_adaptadorTipo == null)
                _adaptadorTipo = _currentAdaptadorTipoFactory.Create();

            return _adaptadorTipo;
        }
        #endregion
    }
}
