
#region Directivas
using System;
#endregion

namespace CorreoFactory
{
    /// <summary>
    /// Clase que representa una fábrica de manejadores de correo
    /// </summary>

    public static class ManejadorCorreosFactory
    {

        #region Miembros

        static IManejadorCorreosFactory _manejadorCorreosFactoryActual = null;

        #endregion

        #region Métodos Estáticos

        /// <summary>
        /// Establece la fábrica de manejadores de correo actual
        /// </summary>
        /// <param name="manejadorCorreosFactory">The manejadorCorreosFactory to set</param>
        public static void SetCurrent(IManejadorCorreosFactory manejadorCorreosFactory)
        {
            _manejadorCorreosFactoryActual = manejadorCorreosFactory;
        }

        /// <summary>
        /// Crea un nuevo manejador de correos con la fábrica actual
        /// </summary>
        /// <returns>Created type adapter</returns>
        public static IManejadorCorreos Create()
        {
            if (_manejadorCorreosFactoryActual == null)
                throw new ApplicationException("No se especificó la fabrica de correo");
            return _manejadorCorreosFactoryActual.Create();
        }

        #endregion

    }
}
