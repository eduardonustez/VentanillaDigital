using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Enumeracion
{
    public enum TablaLogExcepcionEnum
    {
        LogId,
        LogFecha,
        LogCapa,
        LogClase,
        LogMetodo,
        LogModeloOrigen,
        LogTipo,
        LogMensaje,
        LogExcepcion,
        LogDetalleExcepcion,
        LogNombreMaquina,
        LogDireccionIp,
        LogDireccionMac,
        LogUsuario,
        LogTransaccionId
    }
    public enum TablaLogEventoEnum
    {
        LogId,
        LogFecha,
        LogTipo,
        LogMensaje,
        LogTransaccionId,
        LogCapa,
        LogClase,
        LogMetodo,
        LogEntidad,
        LogEntidadId,
        LogDatos,
        LogNombreMaquina,
        LogDireccionIp,
        LogDireccionMac,
        LogUsuario
    }
}
