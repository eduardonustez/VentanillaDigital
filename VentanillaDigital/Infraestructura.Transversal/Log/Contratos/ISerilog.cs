using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Contratos
{
    public interface ISerilog
    {
        void LogInformation(InformationModel informacionPersistida,IdentificacionEquipo identificacionEquipo, string message, 
             params object[] args);
        void LogInformation(string message);
        void LogWarning(InformationModel informacionPersistida, IdentificacionEquipo identificacionEquipo, string message,
             params object[] args);

        void LogError(ErrorModel errorModelo, IdentificacionEquipo identificacionEquipo, string message);
    }
}
