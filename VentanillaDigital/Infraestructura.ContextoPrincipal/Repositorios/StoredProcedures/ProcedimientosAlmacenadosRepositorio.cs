using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.StoredProcedures
{
    public class ProcedimientosAlmacenadosRepositorio : IProcedimientoAlmacenadoRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor

        public ProcedimientosAlmacenadosRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
        #endregion

        #region Esquemas
        private static readonly string TransaccionarSchema = "Transaccional";
        private static readonly string ParametricasSchema = "Parametricas";

        #endregion

        #region Nombre StoredProcedures

        public static readonly string Sp_TramitesPendientes = $"{TransaccionarSchema}.TramitesPendientes_Obtener";

        #endregion

        #region Contratos

        public async Task<Tuple<List<TramitesPendientes>, int>> TramitesPendientes_Obtener(string bodyJson)
        {
            try
            {
                var jsonResult = new StringBuilder();
                int total = 0;

                var connection = _unidadTrabajoContextoPrincipal.Database.GetDbConnection() as SqlConnection;
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand($"{Sp_TramitesPendientes}", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@I_DefinicionFiltro", SqlDbType.NVarChar).Value = bodyJson;
                        cmd.Parameters.Add(new SqlParameter("@O_TotalRegistros", SqlDbType.Int));
                        cmd.Parameters["@O_TotalRegistros"].Direction = ParameterDirection.Output;

                        if (await cmd.ExecuteScalarAsync() is string reader)
                        {
                            jsonResult.Append(reader.ToString());
                        }
                        else
                        {
                            jsonResult.Append("[]");
                        }
                        total = (int)cmd.Parameters["@O_TotalRegistros"].Value;
                    }
                    await connection.CloseAsync();
                }
                var raw = JArray.Parse(jsonResult.ToString());
                var ret = raw.ToObject<List<TramitesPendientes>>();

                return new Tuple<List<TramitesPendientes>, int>(ret, total);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

    }
}
