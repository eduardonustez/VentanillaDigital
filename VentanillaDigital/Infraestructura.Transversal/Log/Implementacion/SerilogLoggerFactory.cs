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
using Infraestructura.Transversal;

namespace Infraestructura.Transversal.Log.Implementacion
{
   public class SerilogLoggerFactory: ISerilogFactory
    {
        private ILogger _logger = null;

        public SerilogLoggerFactory(SerilogConfig configuracionExcepciones,SerilogConfig configuracionEventos)
        {

          

            _logger = new LoggerConfiguration()
                                .Enrich.WithProcessId()
                                .WriteTo.Logger(x => x
                                .MinimumLevel.Verbose()
                                .WriteTo.MSSqlServer(configuracionExcepciones.ConnectionStrings, 
                                tableName: configuracionExcepciones.NombreTabla,
                                schemaName: configuracionExcepciones.NombreSchema, 
                                columnOptions: ObtenerColumnasTablaLogExcepcion(),
                                autoCreateSqlTable: true
                                ).Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level==LogEventLevel.Fatal))
                                
                                .WriteTo.Logger(x => x
                                .MinimumLevel.Verbose().WriteTo.MSSqlServer(configuracionEventos.ConnectionStrings,
                                tableName: configuracionEventos.NombreTabla,
                                schemaName: configuracionEventos.NombreSchema,
                                columnOptions: ObtenerColumnasTablaLogEvento(),
                                autoCreateSqlTable: true
                                ).Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information || e.Level == LogEventLevel.Warning )
                                ).CreateLogger();

        }

        public ISerilog Create()
        {
            return new SerilogLogger(_logger);
        }

        #region MetodosPrivados
        private ColumnOptions ObtenerColumnasTablaLogExcepcion()
        {
            var ColumnasTablaLogExcepcion = new ColumnOptions();

            ColumnasTablaLogExcepcion.Id.ColumnName = TablaLogExcepcionEnum.LogId.ToString();
            ColumnasTablaLogExcepcion.TimeStamp.ColumnName = TablaLogExcepcionEnum.LogFecha.ToString();
            ColumnasTablaLogExcepcion.Level.ColumnName = TablaLogExcepcionEnum.LogTipo.ToString();
            ColumnasTablaLogExcepcion.Message.ColumnName = TablaLogExcepcionEnum.LogMensaje.ToString();
            ColumnasTablaLogExcepcion.Exception.ColumnName = TablaLogExcepcionEnum.LogExcepcion.ToString();
            ColumnasTablaLogExcepcion.Store.Remove(StandardColumn.MessageTemplate);
            ColumnasTablaLogExcepcion.Store.Remove(StandardColumn.Properties);
            ColumnasTablaLogExcepcion.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogTransaccionId.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogDetalleExcepcion.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogCapa.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogClase.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogMetodo.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogModeloOrigen.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogNombreMaquina.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogDireccionIp.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogDireccionMac.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogExcepcionEnum.LogUsuario.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true}
            };
            return ColumnasTablaLogExcepcion;
        }
        private ColumnOptions ObtenerColumnasTablaLogEvento()
        {
            var ColumnasTablaLogExcepcion = new ColumnOptions();
            
            ColumnasTablaLogExcepcion.Id.ColumnName = TablaLogEventoEnum.LogId.ToString();
            ColumnasTablaLogExcepcion.TimeStamp.ColumnName = TablaLogEventoEnum.LogFecha.ToString();
            ColumnasTablaLogExcepcion.Level.ColumnName = TablaLogEventoEnum.LogTipo.ToString();
            ColumnasTablaLogExcepcion.Message.ColumnName = TablaLogEventoEnum.LogMensaje.ToString();
            ColumnasTablaLogExcepcion.Store.Remove(StandardColumn.Exception);
            ColumnasTablaLogExcepcion.Store.Remove(StandardColumn.MessageTemplate);
            ColumnasTablaLogExcepcion.Store.Remove(StandardColumn.Properties);
            ColumnasTablaLogExcepcion.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogTransaccionId.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogCapa.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogClase.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogMetodo.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogEntidad.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogEntidadId.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogDatos.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogNombreMaquina.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogDireccionIp.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogDireccionMac.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true},
                new SqlColumn {ColumnName =TablaLogEventoEnum.LogUsuario.ToString(),DataType = SqlDbType.NVarChar, AllowNull = true}
            };
            return ColumnasTablaLogExcepcion;
        }
        #endregion
    }
}
