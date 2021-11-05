using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace Infraestructura.ContextoPrincipal.Repositorios.Transaccional
{
    public class DocumentoPendienteAutorizarRepositorio : RepositorioBase<DocumentoPendienteAutorizar>, IDocumentoPendienteAutorizarRepositorio
    {
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        public DocumentoPendienteAutorizarRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        public async Task<IEnumerable<DocumentoPendienteAutorizar>> ObtenerProximas(int cantidad)
        {
            var query = (from d in _unidadTrabajoContextoPrincipal.DocumentosPendienteAutorizar where d.Estado == EstadoDocumento.PENDIENTE select d).Take(cantidad);
            //var query = (from d in _unidadTrabajoContextoPrincipal.DocumentosPendienteAutorizar where d.Estado == EstadoDocumento.ERROR && d.DocumentoPendienteAutorizarId >= 97 && d.DocumentoPendienteAutorizarId <= 106 select d);

            return await query.ToListAsync();
        }

        public async Task<List<long>> ObtenerProximasSpAsync(int cantidad)
        {
            try
            {
                using (var cnn = _unidadTrabajoContextoPrincipal.Database.GetDbConnection())
                {
                    var cmm = cnn.CreateCommand();
                    cmm.CommandType = CommandType.StoredProcedure;
                    cmm.CommandText = "Transaccional.ObtenerDocumentosPendientesAutorizar";

                    var parameter = cmm.CreateParameter();
                    parameter.ParameterName = "@Cantidad";
                    parameter.DbType = DbType.Int32;
                    parameter.Value = cantidad;

                    cmm.Parameters.Add(parameter);

                    parameter = cmm.CreateParameter();
                    parameter.ParameterName = "@MachineName";
                    parameter.DbType = DbType.String;
                    parameter.Value = Environment.MachineName;

                    cmm.Parameters.Add(parameter);

                    cmm.Connection = cnn;
                    cnn.Open();
                    var reader = await cmm.ExecuteReaderAsync();

                    var tb = new DataTable();
                    tb.Load(reader);

                    await reader.DisposeAsync();

                    var result = new List<long>();

                    for (int i = 0; i < tb.Rows.Count; i++) result.Add(Convert.ToInt64(tb.Rows[i][0]));

                    return result;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
