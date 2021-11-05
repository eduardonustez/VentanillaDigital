using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Excepcion
{
    public class ApplicationValidationErrorsException:Exception
    {
        IEnumerable<string> _validationErrors;
        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }
        public ApplicationValidationErrorsException(IEnumerable<string> validationErrors)
        {
            _validationErrors = validationErrors;
        }
    }
}
