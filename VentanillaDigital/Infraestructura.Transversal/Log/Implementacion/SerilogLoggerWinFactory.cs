using Infraestructura.Transversal.Log.Contratos;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using Serilog.Events;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Modelo;
using Serilog.Sinks.EventLog;

namespace Infraestructura.Transversal.Log.Implementacion
{
   public class SerilogLoggerWinFactory: ISerilogFactory
    {
        private ILogger _logger = null;

        public SerilogLoggerWinFactory(string applicationName)
        {

            _logger = new LoggerConfiguration()
               .WriteTo.EventLog(applicationName,
                      manageEventSource: true)
                .CreateLogger();

        }

        public ISerilog Create()
        {
            return new SerilogLogger(_logger);
        }

       
    }
}
