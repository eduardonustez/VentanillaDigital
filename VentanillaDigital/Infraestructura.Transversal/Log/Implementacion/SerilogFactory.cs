using Infraestructura.Transversal.Log.Contratos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Implementacion
{
    public class SerilogFactory
    {
        static ISerilogFactory _currentSerilogFactory = null;
        static ISerilog _currentSerilog = null;

        #region Public Metodos

        public static void SetCurrent(ISerilogFactory serilogFactory)
        {
            _currentSerilogFactory = serilogFactory;
        }


        public static ISerilog Create()
        {
            if (_currentSerilogFactory == null)
                throw new ApplicationException(Mensaje.Recursos.Excepcion_SerilogFactory);

            if (_currentSerilog == null)
                _currentSerilog = _currentSerilogFactory.Create();

            return _currentSerilog;
        }

        #endregion
    }
}
