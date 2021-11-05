using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class TramiteRepositorio : RepositorioBase<Tramite>, ITramiteRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public TramiteRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
        #endregion

        #region Contratos
        public async Task<bool> NotariaActiva(long NotariaId)
        {
            bool esNotariaActiva = await _unidadTrabajoContextoPrincipal.Notaria.Where(x => x.IsDeleted == false && x.NotariaId == NotariaId).AnyAsync().ConfigureAwait(false);

            return esNotariaActiva;
        }

        public async Task<bool> TipoTramiteActivo(long TipoTramiteId)
        {
            bool esNotariaActiva = await _unidadTrabajoContextoPrincipal.TipoTramite.Where(x => x.IsDeleted == false && x.TipoTramiteId == TipoTramiteId).AnyAsync().ConfigureAwait(false);

            return esNotariaActiva;
        }

        public async Task<EstadoTramite> ObtenerEstadoTramite(string Nombre)
        {
            EstadoTramite respuestaEstado = new EstadoTramite();
            var obtenerEstadoTramite = await _unidadTrabajoContextoPrincipal.EstadoTramite.Where(x => x.Nombre == Nombre).AnyAsync().ConfigureAwait(false);
            if (obtenerEstadoTramite)
            {
                respuestaEstado = await _unidadTrabajoContextoPrincipal.EstadoTramite.Where(x => x.Nombre == Nombre).FirstOrDefaultAsync();
            }
            return respuestaEstado;
        }
        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitesPaginado(DefinicionFiltro definicionFiltro)
        {

            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;

            await _unidadTrabajoContextoPrincipal.Database
                .ExecuteSqlRawAsync("EXEC [Transaccional].[Tramites_Obtener2] @I_DefinicionFiltro, @O_TotalRegistros OUTPUT, @O_Resultado OUTPUT",
                new SqlParameter("@I_DefinicionFiltro", JsonConvert.SerializeObject(definicionFiltro)), oTotalRegistros, oResultado);


            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                Respuesta.Resultado = oResultado.Value.ToString();
                Respuesta.TotalRegistros = oTotalRegistros.Value is DBNull ? 0 : (long)oTotalRegistros.Value;
                Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)definicionFiltro.RegistrosPagina);
            }
            else
            {
                Respuesta.Resultado = "[]";
                Respuesta.TotalRegistros = 0;
            }
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }


        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitesPendientesAutPaginado(FiltroTramites filtroTramites)
        {
            TimeSpan ti = new TimeSpan(00, 00, 0);
            TimeSpan tf = new TimeSpan(23, 59, 0);
            if (filtroTramites.fechaInicial == null)
            {
                filtroTramites.fechaInicial = DateTime.Now.AddDays(-8);
                filtroTramites.fechaInicial = filtroTramites.fechaInicial.Value.Date + ti;
            }


            if (filtroTramites.fechaFinal == null)
            {
                filtroTramites.fechaFinal = DateTime.Now;
                filtroTramites.fechaFinal = filtroTramites.fechaFinal.Value.Date + tf;
            }


            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;


            var NotariaId = new SqlParameter
            {
                ParameterName = "NotariaId",
                Value = filtroTramites.NotariaId,
                Direction = ParameterDirection.Input
            };



            var fechaInicial = new SqlParameter
            {
                ParameterName = "fechaInicial",
                Value = filtroTramites.fechaInicial,
                Direction = ParameterDirection.Input
            };


            var fechaFinal = new SqlParameter
            {
                ParameterName = "fechaFinal",
                Value = filtroTramites.fechaFinal,
                Direction = ParameterDirection.Input
            };


            var PageNumber = new SqlParameter
            {
                ParameterName = "PageNumber",
                Value = filtroTramites.NumeroPagina,
                Direction = ParameterDirection.Input
            };

            var RowspPage = new SqlParameter
            {
                ParameterName = "RowspPage",
                Value = filtroTramites.RegistrosPagina,
                Direction = ParameterDirection.Input
            };

            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;


            var SQL = "EXEC [Transaccional].[Tramites_PendientesAutorizacion] @NotariaId, @FechaInicial, @Fechafinal, @PageNumber, @RowspPage, @O_Resultado OUTPUT, @O_TotalRegistros OUTPUT";
            var result = _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRaw(SQL, NotariaId, fechaInicial, fechaFinal, PageNumber, RowspPage, oResultado, oTotalRegistros);


            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                Respuesta.Resultado = oResultado.Value.ToString();
                Respuesta.TotalRegistros = (long)oTotalRegistros.Value;
                Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)filtroTramites.RegistrosPagina);
            }
            else
            {
                Respuesta.Resultado = "[]";
                Respuesta.TotalRegistros = 0;
            }
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }

        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitesAutorizadoPaginado(FiltroTramites filtroTramites)
        {
            SqlParameter NuipOperador = new SqlParameter();
            SqlParameter NuipCompareciente = new SqlParameter();
            SqlParameter TramiteId = new SqlParameter();

            if (filtroTramites.fechaInicial == null)
            {
                filtroTramites.fechaInicial = DateTime.Now.AddDays(-8);
            }


            if (filtroTramites.fechaFinal == null)
            {
                filtroTramites.fechaFinal = DateTime.Now;
            }


            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;


            var NotariaId = new SqlParameter
            {
                ParameterName = "NotariaId",
                Value = filtroTramites.NotariaId,
                Direction = ParameterDirection.Input
            };



            var fechaInicial = new SqlParameter
            {
                ParameterName = "fechaInicial",
                Value = filtroTramites.fechaInicial,
                Direction = ParameterDirection.Input
            };


            var fechaFinal = new SqlParameter
            {
                ParameterName = "fechaFinal",
                Value = filtroTramites.fechaFinal,
                Direction = ParameterDirection.Input
            };

            if (filtroTramites.NuipOperador == null)
            {
                NuipOperador.ParameterName = "NuipOperador";
                NuipOperador.Value = DBNull.Value;
                NuipOperador.Direction = ParameterDirection.Input;
            }
            else
            {
                NuipOperador.ParameterName = "NuipOperador";
                NuipOperador.Value = filtroTramites.NuipOperador;
                NuipOperador.Direction = ParameterDirection.Input;
            }

            if (filtroTramites.TramiteId == 0)
            {
                TramiteId.ParameterName = "TramiteId";
                TramiteId.Value = DBNull.Value;
                TramiteId.Direction = ParameterDirection.Input;
            }
            else
            {
                TramiteId.ParameterName = "TramiteId";
                TramiteId.Value = filtroTramites.TramiteId;
                TramiteId.Direction = ParameterDirection.Input;
            }

            if (filtroTramites.NuipCompareciente == null)
            {
                NuipCompareciente.ParameterName = "NuipCompareciente";
                NuipCompareciente.Value = DBNull.Value;
                NuipCompareciente.Direction = ParameterDirection.Input;
            }
            else
            {
                NuipCompareciente.ParameterName = "NuipCompareciente";
                NuipCompareciente.Value = filtroTramites.NuipCompareciente;
                NuipCompareciente.Direction = ParameterDirection.Input;
            }

            var PageNumber = new SqlParameter
            {
                ParameterName = "PageNumber",
                Value = filtroTramites.NumeroPagina,
                Direction = ParameterDirection.Input
            };

            var RowspPage = new SqlParameter
            {
                ParameterName = "RowspPage",
                Value = filtroTramites.RegistrosPagina,
                Direction = ParameterDirection.Input
            };

            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;


            //var SQL = "EXEC [Transaccional].[Tramites_Autorizados] @NotariaId, @FechaInicial, @Fechafinal, @NuipOperador, @NuipCompareciente, @PageNumber, @RowspPage, @O_Resultado OUTPUT, @O_TotalRegistros OUTPUT";
            //var result = _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRaw(SQL, NotariaId, fechaInicial, fechaFinal, NuipOperador, NuipCompareciente, PageNumber, RowspPage, oResultado, oTotalRegistros);

            var SQL = "EXEC [Transaccional].[Tramites_Autorizados] @NotariaId, @TramiteId, @FechaInicial, @Fechafinal, @NuipOperador , @NuipCompareciente, @PageNumber, @RowspPage, @O_Resultado OUTPUT, @O_TotalRegistros OUTPUT";
            var result = _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRaw(SQL, NotariaId, TramiteId, fechaInicial, fechaFinal, NuipOperador, NuipCompareciente, PageNumber, RowspPage, oResultado, oTotalRegistros);



            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                Respuesta.Resultado = oResultado.Value.ToString();
                Respuesta.TotalRegistros = (long)oTotalRegistros.Value;
                Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)filtroTramites.RegistrosPagina);
            }
            else
            {
                Respuesta.Resultado = "[]";
                Respuesta.TotalRegistros = 0;
            }
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }

        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitesEnProcesoPaginado(FiltroTramites filtroTramites)
        {
            SqlParameter NuipOperador = new SqlParameter();
            if (filtroTramites.fechaInicial == null)
            {
                filtroTramites.fechaInicial = DateTime.Now.AddDays(-8);
            }


            if (filtroTramites.fechaFinal == null)
            {
                filtroTramites.fechaFinal = DateTime.Now;
            }


            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;


            var NotariaId = new SqlParameter
            {
                ParameterName = "NotariaId",
                Value = filtroTramites.NotariaId,
                Direction = ParameterDirection.Input
            };



            var fechaInicial = new SqlParameter
            {
                ParameterName = "fechaInicial",
                Value = filtroTramites.fechaInicial,
                Direction = ParameterDirection.Input
            };


            var fechaFinal = new SqlParameter
            {
                ParameterName = "fechaFinal",
                Value = filtroTramites.fechaFinal,
                Direction = ParameterDirection.Input
            };

            if (filtroTramites.NuipOperador == null)
            {
                NuipOperador.ParameterName = "UsuarioCreador";
                NuipOperador.Value = DBNull.Value;
                NuipOperador.Direction = ParameterDirection.Input;
            }
            else
            {
                NuipOperador.ParameterName = "UsuarioCreador";
                NuipOperador.Value = filtroTramites.NuipOperador;
                NuipOperador.Direction = ParameterDirection.Input;
            }


            var PageNumber = new SqlParameter
            {
                ParameterName = "PageNumber",
                Value = filtroTramites.NumeroPagina,
                Direction = ParameterDirection.Input
            };

            var RowspPage = new SqlParameter
            {
                ParameterName = "RowspPage",
                Value = filtroTramites.RegistrosPagina,
                Direction = ParameterDirection.Input
            };

            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;


            var SQL = "EXEC [Transaccional].[Tramites_enProceso] @NotariaId, @FechaInicial, @Fechafinal, @UsuarioCreador,  @PageNumber, @RowspPage, @O_Resultado OUTPUT, @O_TotalRegistros OUTPUT";
            var result = _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRaw(SQL, NotariaId, fechaInicial, fechaFinal, NuipOperador, PageNumber, RowspPage, oResultado, oTotalRegistros);


            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                Respuesta.Resultado = oResultado.Value.ToString();
                Respuesta.TotalRegistros = (long)oTotalRegistros.Value;
                Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)filtroTramites.RegistrosPagina);
            }
            else
            {
                Respuesta.Resultado = "[]";
                Respuesta.TotalRegistros = 0;
            }
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }

        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitesRechazadosPaginado(FiltroTramites filtroTramites)
        {
            SqlParameter TramiteId = new SqlParameter();
            SqlParameter NuipOperador = new SqlParameter();
            SqlParameter NuipCompareciente = new SqlParameter();


            if (filtroTramites.fechaInicial == null)
            {
                filtroTramites.fechaInicial = DateTime.Now.AddDays(-368);
            }


            if (filtroTramites.fechaFinal == null)
            {
                filtroTramites.fechaFinal = DateTime.Now;
            }


            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;


            var NotariaId = new SqlParameter
            {
                ParameterName = "NotariaId",
                Value = filtroTramites.NotariaId,
                Direction = ParameterDirection.Input
            };



            var fechaInicial = new SqlParameter
            {
                ParameterName = "fechaInicial",
                Value = filtroTramites.fechaInicial,
                Direction = ParameterDirection.Input
            };


            var fechaFinal = new SqlParameter
            {
                ParameterName = "fechaFinal",
                Value = filtroTramites.fechaFinal,
                Direction = ParameterDirection.Input
            };

            if (filtroTramites.NuipOperador == null)
            {
                NuipOperador.ParameterName = "NuipOperador";
                NuipOperador.Value = DBNull.Value;
                NuipOperador.Direction = ParameterDirection.Input;
            }
            else
            {
                NuipOperador.ParameterName = "NuipOperador";
                NuipOperador.Value = filtroTramites.NuipOperador;
                NuipOperador.Direction = ParameterDirection.Input;
            }


            if (filtroTramites.NuipCompareciente == null)
            {
                NuipCompareciente.ParameterName = "NuipCompareciente";
                NuipCompareciente.Value = DBNull.Value;
                NuipCompareciente.Direction = ParameterDirection.Input;
            }
            else
            {
                NuipCompareciente.ParameterName = "NuipCompareciente";
                NuipCompareciente.Value = filtroTramites.NuipCompareciente;
                NuipCompareciente.Direction = ParameterDirection.Input;
            }


            if (filtroTramites.TramiteId == 0)
            {
                TramiteId.ParameterName = "TramiteId";
                TramiteId.Value = DBNull.Value;
                TramiteId.Direction = ParameterDirection.Input;
            }
            else
            {
                TramiteId.ParameterName = "TramiteId";
                TramiteId.Value = filtroTramites.TramiteId;
                TramiteId.Direction = ParameterDirection.Input;
            }

            var PageNumber = new SqlParameter
            {
                ParameterName = "PageNumber",
                Value = filtroTramites.NumeroPagina,
                Direction = ParameterDirection.Input
            };

            var RowspPage = new SqlParameter
            {
                ParameterName = "RowspPage",
                Value = filtroTramites.RegistrosPagina,
                Direction = ParameterDirection.Input
            };


            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;


            var SQL = "EXEC [Transaccional].[Tramites_Rechazados] @NotariaId, @TramiteId, @FechaInicial, @Fechafinal, @NuipOperador,@NuipCompareciente,  @PageNumber, @RowspPage, @O_Resultado OUTPUT, @O_TotalRegistros OUTPUT";
            var result = _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRaw(SQL, NotariaId, TramiteId, fechaInicial, NuipOperador, NuipCompareciente, fechaFinal, PageNumber, RowspPage, oResultado, oTotalRegistros);


            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                Respuesta.Resultado = oResultado.Value.ToString();
                Respuesta.TotalRegistros = (long)oTotalRegistros.Value;
                Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)filtroTramites.RegistrosPagina);
            }
            else
            {
                Respuesta.Resultado = "[]";
                Respuesta.TotalRegistros = 0;
            }
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }



        public Task<Tramite> ObtenerTramiteActa(long tramiteId, string tipoActa)
        {
            IQueryable<Tramite> q = base.ObtenerTodo()
                .Where(t => t.IsDeleted == false &&
                       t.TramiteId == tramiteId)
                .Include(t => t.ActaNotarial.Archivo);
            if (tipoActa == "PlantillaSticker")
                q = q.Include(t => t.TipoTramite.PlantillaSticker);
            else
                q = q.Include(t => t.TipoTramite.PlantillaActa);

            return q.FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Tramite>> ObtenerTramitesActa(IEnumerable<long> tramitesId,
            string tipoActa)
        {

            IQueryable<Tramite> q = base.ObtenerTodo()
                .Where(t => t.IsDeleted == false &&
                       tramitesId.Contains(t.TramiteId))
                .Include(t => t.ActaNotarial.Archivo);
            if (tipoActa == "PlantillaSticker")
                q = q.Include(t => t.TipoTramite.PlantillaSticker);
            else
                q = q.Include(t => t.TipoTramite.PlantillaActa);

            return q.ToListAsync().ContinueWith(t => (IEnumerable<Tramite>)t.Result);
        }
        public async Task<DatosTramiteResumen> ObtenerDatosTramiteResumen(long tramiteId)
        {
            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            await _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRawAsync("[Transaccional].[ObtenerTramiteActaResumen] @tramiteId, @O_Resultado OUTPUT",
                new SqlParameter("@tramiteId", tramiteId), oResultado);

            if (string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    var resul = JsonConvert.DeserializeObject<DatosTramiteResumen[]>(oResultado.Value.ToString());
                    return resul[0];
                }
                catch
                {
                    return null;
                }
            }

        }

        #endregion
    }
}
