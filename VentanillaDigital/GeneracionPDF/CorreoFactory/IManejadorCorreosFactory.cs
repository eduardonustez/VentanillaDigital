#region Directivas



#endregion

using System.Collections.Generic;

namespace CorreoFactory
{

    /// <summary>
    /// Contrato base para la fábrica de manejadores de correo
    /// </summary>
    public interface IManejadorCorreosFactory
    {

        /// <summary>
        /// Crea un nuevo manejador de eventos
        /// </summary>
        /// <returns>El manejador de eventos creado</returns>
        IManejadorCorreos Create();

        

    }
}
