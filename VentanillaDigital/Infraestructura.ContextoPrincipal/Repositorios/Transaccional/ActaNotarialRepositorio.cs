using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Infraestructura.Transversal.ExtensionMethods;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Infraestructura.ContextoPrincipal.Repositorios
{

    public class ActaNotarialRepositorio : RepositorioBase<MetadataArchivo>, IActaNotarialRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public ActaNotarialRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        public async Task<InfoActa> ObtenerInformacionPrincipalActaNotarial(long tramiteId, string notarioUser)
        {
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted,
            }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var query = (from tr in _unidadTrabajoContextoPrincipal.Tramite
                             join tt in _unidadTrabajoContextoPrincipal.TipoTramite on tr.TipoTramiteId equals tt.TipoTramiteId
                             join nu in _unidadTrabajoContextoPrincipal.NotariaUsuarios on new { tr.NotariaId, UserEmail = notarioUser } equals new { nu.NotariaId, nu.UserEmail }
                             join p in _unidadTrabajoContextoPrincipal.Persona on nu.PersonaId equals p.PersonaId
                             //join n in _unidadTrabajoContextoPrincipal.Notaria on nu.NotariaId equals n.NotariaId // cachear
                             //join u in _unidadTrabajoContextoPrincipal.Ubicaciones on n.UbicacionId equals u.UbicacionId
                             join nt in _unidadTrabajoContextoPrincipal.Notario on nu.NotariaUsuariosId equals nt.NotariaUsuariosId
                             //join u2 in _unidadTrabajoContextoPrincipal.Ubicaciones on u.UbicacionPadreId equals u2.UbicacionId into lju
                             //from dju in lju.DefaultIfEmpty()
                             where tr.TramiteId == tramiteId
                             select new InfoActa
                             {
                                 TramiteId = tr.TramiteId,
                                 TipoTramite = tt.Nombre,
                                 //Municipio = u.Nombre,
                                 //Departamento = string.IsNullOrEmpty(dju.Nombre) ? "" : string.Concat(", Departamento de ", dju.Nombre),
                                 FechaTramite = tr.Fecha.ConvertirALetras(),
                                 //NumeroNotaria = n.NumeroNotaria == 0 ? "" : $"({n.NumeroNotaria})",
                                 //NumeroNotariaEnLetra = n.NumeroNotariaEnLetras,
                                 //NumeroNotarioEnLetra = n.NumeroNotariaEnLetras,
                                 //CirculoNotaria = n.CirculoNotaria,
                                 NombreNotario = string.Concat(p.Nombres, " ", p.Apellidos),
                                 //UrlQR = parametricas ?? "",
                                 TipoNotario = nt.TipoNotario == 1 ? "" : "Encargado",
                                 NUT = tr.TramiteId,
                                 iddoc = tr.TramiteId,
                                 GeneroNotario = "Notario"
                                 //DatosAdicionales = tr.DatosAdicionales
                             });

                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<ComparecienteData>> ObtenerComparecientes(long tramiteid)
        {
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted,
            }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var query = (from c in _unidadTrabajoContextoPrincipal.Compareciente
                                 //join t in _unidadTrabajoContextoPrincipal.Tramite on c.TramiteId equals t.TramiteId
                             join p in _unidadTrabajoContextoPrincipal.Persona on c.PersonaId equals p.PersonaId
                             join ma1 in _unidadTrabajoContextoPrincipal.MetadataArchivos on c.FotoId equals ma1.MetadataArchivoId into ljma1
                             from dma1 in ljma1.DefaultIfEmpty()
                             join fo in _unidadTrabajoContextoPrincipal.Archivos on dma1.MetadataArchivoId equals fo.MetadataArchivoId into ljfo
                             from dfo in ljfo.DefaultIfEmpty()
                             join ma2 in _unidadTrabajoContextoPrincipal.MetadataArchivos on c.FirmaId equals ma2.MetadataArchivoId into ljma2
                             from dma2 in ljma2.DefaultIfEmpty()
                             join fo2 in _unidadTrabajoContextoPrincipal.Archivos on dma2.MetadataArchivoId equals fo2.MetadataArchivoId into ljfo2
                             from dfo2 in ljfo2.DefaultIfEmpty()
                             join td in _unidadTrabajoContextoPrincipal.TiposIdentificacion on p.TipoIdentificacionId equals td.TipoIdentificacionId into ltd
                             from dtd in ltd.DefaultIfEmpty()
                             where c.TramiteId == tramiteid
                             select new ComparecienteData
                             {
                                 ComparecienteId = c.ComparecienteId,
                                 NombreCompareciente = string.Concat(p.Nombres, " ", p.Apellidos),
                                 NUIPCompareciente = p.NumeroDocumento,
                                 FotoCompareciente = dfo.Contenido,
                                 FirmaCompareciente = dfo2.Contenido,
                                 NUT = c.PeticionRNEC,
                                 //FechaCompletaNumeros = t.Fecha.ToString("dd/MM/yyyy"),
                                 //HoraCompletaNumeros = t.Fecha.ToString("hh:mm:ss"),
                                 //TextoBiometria = c.TramiteSinBiometria ? string.Concat(textoSinBiometria, " ", c.MotivoSinBiometria) : textoConBiometria
                                 TramiteSinBiometria = c.TramiteSinBiometria,
                                 MotivoSinBiometria = c.MotivoSinBiometria,
                                 NombreTipoDocumento = dtd.Nombre
                             });

                return await query.ToListAsync();
            }
        }

        public string[] ObtenerCamposActaNotarial(long TramiteId, string notarioUser)
        {
            string[] result;
            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            var oResultadoDatosAdicionales = new SqlParameter("@O_ResultadoDatosAdicionales", SqlDbType.NVarChar);
            oResultadoDatosAdicionales.SqlDbType = SqlDbType.NVarChar;
            oResultadoDatosAdicionales.Size = int.MaxValue;
            oResultadoDatosAdicionales.Direction = ParameterDirection.Output;
            oResultadoDatosAdicionales.DbType = DbType.String;

            var OResultadoComparecientes = new SqlParameter("@O_ResultadoComparecientes", SqlDbType.NVarChar);
            OResultadoComparecientes.SqlDbType = SqlDbType.NVarChar;
            OResultadoComparecientes.Size = int.MaxValue;
            OResultadoComparecientes.Direction = ParameterDirection.Output;
            OResultadoComparecientes.DbType = DbType.String;

            _unidadTrabajoContextoPrincipal.Database
                .ExecuteSqlRaw("EXEC [Transaccional].[ObtenerCamposActaNotarialV2] @tramiteId,@notarioUser, @O_Resultado OUTPUT, @O_ResultadoComparecientes OUTPUT, @O_ResultadoDatosAdicionales OUTPUT",
                new SqlParameter("@tramiteId", TramiteId), new SqlParameter("@notarioUser", notarioUser), oResultado, OResultadoComparecientes, oResultadoDatosAdicionales);


            if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                result = new string[] { oResultado.Value.ToString(), OResultadoComparecientes.Value.ToString(),
                    oResultadoDatosAdicionales.Value.ToString() };
                return result;
            }

            else
                return new string[] { };
        }

        public async Task<DatosNotarioFirmaManual> ObtenerDatosNotarioFirmaManual(long tramiteId)
        {
            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            await _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRawAsync("[Transaccional].[ObtenerNotarioActaFirmaManual] @notariaId, @O_Resultado OUTPUT",
                new SqlParameter("@notariaId", tramiteId), oResultado);

            if (string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    var resul = JsonConvert.DeserializeObject<DatosNotarioFirmaManual[]>(oResultado.Value.ToString());
                    return resul[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public async Task<ArchivoActa> ObtenerActa(long tramiteId)
        {
            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            await _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRawAsync("[Transaccional].[Actas_Obtener] @tramiteId, @O_Resultado OUTPUT",
                new SqlParameter("@tramiteId", tramiteId), oResultado);

            if (string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    var resul = JsonConvert.DeserializeObject<ArchivoActa[]>(oResultado.Value.ToString());
                    return resul[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<ArchivoActa> ObtenerActa(long tramiteId, long notariaId, DateTime fecha)
        {
            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            await _unidadTrabajoContextoPrincipal.Database.ExecuteSqlRawAsync
            (
                "[Transaccional].[Actas_ObtenerNS] @tramiteId, @notariaId, @fechaTramite, @O_Resultado OUTPUT",
                new SqlParameter("@tramiteId", tramiteId),
                new SqlParameter("@notariaId", notariaId),
                new SqlParameter("@fechaTramite", fecha), oResultado
            );

            if (string.IsNullOrEmpty(oResultado.Value.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    var resul = JsonConvert.DeserializeObject<ArchivoActa[]>(oResultado.Value.ToString());
                    return resul[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<Parametrica> ObtenerParametrica(string valor)
            => await (from t in _unidadTrabajoContextoPrincipal.Parametrica where t.Codigo.Equals(valor) select t).FirstOrDefaultAsync();
        #endregion

    }
}
