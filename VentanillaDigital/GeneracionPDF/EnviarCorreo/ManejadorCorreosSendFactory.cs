#region Directivas

using CorreoFactory;

#endregion

namespace Generacion_PDF_Notaria.EnviarCorreo
{
    /// <summary>
    /// Clase que implementa la fábrica de manejadores de correos con SendGrid.
    /// </summary>
    public class ManejadorCorreosSendFactory : IManejadorCorreosFactory
    {

        #region IManejadorCorreosFactory Members

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public IManejadorCorreos Create()
        {
            return new ManejadorCorreos();
        }

        #endregion

    }
}
