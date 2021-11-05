using Infraestructura.Transversal.Log.Contratos;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Log.Enumeracion;

namespace Infraestructura.Transversal.Log.Implementacion
{
    public class SerilogLogger : ISerilog
    {
        private ILogger _logger = null;
        
        public SerilogLogger(ILogger logger)
        {
            _logger = logger;
        }

    
        public void LogInformation( InformationModel informacionPersitida,IdentificacionEquipo identificacionEquipo,  string message,
             params object[] args)
        {
            _logger.ForContext(TablaLogEventoEnum.LogNombreMaquina.ToString(), identificacionEquipo.NombreEquipo)
                   .ForContext(TablaLogEventoEnum.LogDireccionIp.ToString(), identificacionEquipo.DireccionIp)
                   .ForContext(TablaLogEventoEnum.LogDireccionMac.ToString(), identificacionEquipo.DireccionMac)
                   .ForContext(TablaLogEventoEnum.LogTransaccionId.ToString(), informacionPersitida.TransaccionId)
                    .ForContext(TablaLogEventoEnum.LogCapa.ToString(), informacionPersitida.Capa)
                   .ForContext(TablaLogEventoEnum.LogClase.ToString(), informacionPersitida.Clase)
                   .ForContext(TablaLogEventoEnum.LogMetodo.ToString(), informacionPersitida.Metodo)
                   .ForContext(TablaLogEventoEnum.LogEntidad.ToString(), informacionPersitida.Entidad)
                    .ForContext(TablaLogEventoEnum.LogEntidadId.ToString(), informacionPersitida.EntidadId)
                   .ForContext(TablaLogEventoEnum.LogDatos.ToString(), informacionPersitida.Datos)
                    .ForContext(TablaLogEventoEnum.LogUsuario.ToString(), informacionPersitida.Usuario)
                   .Information(message, args);
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogError(ErrorModel errorModelo, IdentificacionEquipo identificacionEquipo,string message)
        {
            
            _logger.ForContext(
                TablaLogExcepcionEnum.LogDetalleExcepcion.ToString(), errorModelo.exception.ToString())
                .ForContext(TablaLogExcepcionEnum.LogExcepcion.ToString(),errorModelo.exception.Message)
                .ForContext(TablaLogExcepcionEnum.LogNombreMaquina.ToString(), identificacionEquipo.NombreEquipo)
                   .ForContext(TablaLogExcepcionEnum.LogDireccionIp.ToString(),identificacionEquipo.DireccionIp)
                   .ForContext(TablaLogExcepcionEnum.LogDireccionMac.ToString(), identificacionEquipo.DireccionMac)
                   .ForContext(TablaLogExcepcionEnum.LogCapa.ToString(), errorModelo.Capa)
                   .ForContext(TablaLogExcepcionEnum.LogClase.ToString(), errorModelo.Clase)
                    .ForContext(TablaLogExcepcionEnum.LogMetodo.ToString(), errorModelo.Metodo)
                   .ForContext(TablaLogExcepcionEnum.LogModeloOrigen.ToString(), errorModelo.Modelo)
                    .ForContext(TablaLogExcepcionEnum.LogTransaccionId.ToString(), errorModelo.TransaccionId.ToString())
                    .ForContext(TablaLogExcepcionEnum.LogUsuario.ToString(),errorModelo.Usuario)
                   .Error(message);


        }
        public void LogWarning(InformationModel informacionPersitida, IdentificacionEquipo identificacionEquipo,string message,
              params object[] args)
        {
            _logger.ForContext(TablaLogEventoEnum.LogNombreMaquina.ToString(), identificacionEquipo.NombreEquipo)
                   .ForContext(TablaLogEventoEnum.LogDireccionIp.ToString(), identificacionEquipo.DireccionIp)
                   .ForContext(TablaLogEventoEnum.LogDireccionMac.ToString(), identificacionEquipo.DireccionMac)
                   .ForContext(TablaLogEventoEnum.LogTransaccionId.ToString(), informacionPersitida.TransaccionId)
                    .ForContext(TablaLogEventoEnum.LogCapa.ToString(), informacionPersitida.Capa)
                   .ForContext(TablaLogEventoEnum.LogClase.ToString(), informacionPersitida.Clase)
                   .ForContext(TablaLogEventoEnum.LogMetodo.ToString(), informacionPersitida.Metodo)
                   .ForContext(TablaLogEventoEnum.LogEntidad.ToString(), informacionPersitida.Entidad)
                    .ForContext(TablaLogEventoEnum.LogEntidadId.ToString(), informacionPersitida.EntidadId)
                   .ForContext(TablaLogEventoEnum.LogDatos.ToString(), informacionPersitida.Datos)
                    .ForContext(TablaLogEventoEnum.LogUsuario.ToString(), informacionPersitida.Usuario)
                   .Warning(message, args);
        }
    }
}
