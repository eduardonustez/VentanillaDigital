using System;
using Aplicacion.Nucleo.Base;
using System.Threading.Tasks;
using System.Linq;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using GenericExtensions;
using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Enumeracion;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;

namespace plicacion.ContextoPrincipal.Servicio
{
    public class MaquinaServicio : BaseServicio, IMaquinaServicio
    {
        #region Const - Mensajes
        private const string NOTARIAINCORRECTA = "El número de notaria es incorrecta";
        private const string TIPOMAQUINAINCORRECTO = "El tipo de Maquina es incorrecto";
        private const string NOMBREMAQUINAEXISTENTE = "Nombre de Máquina ya existe";
        private const string MACMAQUINAEXISTENTE = "MAC de la Máquina ya asignada a la Notaría";
        private const string ERRORSISTEMA = "Error al validar por favor intente de nuevo, si el error persiste por favor comunicarse con el administrador";

        #endregion

        #region Propiedades
        private IMaquinaRepositorio _MaquinaRepositorio { get; }
        private IConvenioRNECRepositorio _ConvenioRNECRepositorio { get; }
        public IProcedimientoAlmacenadoRepositorio _procedimientoAlmacenado { get; }

        #endregion

        #region Constructor
        public MaquinaServicio(
            IMaquinaRepositorio MaquinaRepositorio
            , IProcedimientoAlmacenadoRepositorio procedimientoAlmacenadoRepositorio
            , IConvenioRNECRepositorio convenioRNECRepositorio) : base(MaquinaRepositorio)
        {
            _MaquinaRepositorio = MaquinaRepositorio;
            _procedimientoAlmacenado = procedimientoAlmacenadoRepositorio;
            _ConvenioRNECRepositorio = convenioRNECRepositorio;
        }

        #endregion

        #region Contratos


        public async Task<MaquinaDTO> CrearMaquina(MaquinaCreateDTO MaquinaDto)
        {
            MaquinaDTO MaquinaReturn;

            MaquinaReturn = await ValidarRequestCreacionMaquina(MaquinaDto);


            if (MaquinaReturn.EsValido)
            {
                var MaquinaExistente = await _MaquinaRepositorio.GetOneAsync(x => x.MAC.Trim() == MaquinaDto.MAC.Trim());
                if (MaquinaExistente!=null)
                {
                    MaquinaExistente.UsuarioModificacion = MaquinaDto.UserName;
                    MaquinaExistente.FechaModificacion = DateTime.Now;
                    MaquinaExistente.DireccionIP = MaquinaDto.DireccionIP;
                    MaquinaExistente.IsDeleted = false;
                    MaquinaExistente.CanalWacom = MaquinaDto.CanalWacom;
                    MaquinaExistente.EstadoWacomSigCaptX = MaquinaDto.EstadoWacomSigCaptX;
                    MaquinaExistente.EstadoDllWacom = MaquinaDto.EstadoDllWacom;
                    MaquinaExistente.EstadoCaptor = MaquinaDto.EstadoCaptor;
                    _MaquinaRepositorio.Modificar(MaquinaExistente);
                    _MaquinaRepositorio.UnidadDeTrabajo.Commit();
                    MaquinaReturn.MaquinaId = MaquinaExistente.MaquinaId;
                    MaquinaReturn.DireccionIP = MaquinaExistente.DireccionIP;
                    MaquinaReturn.MAC = MaquinaExistente.MAC;
                    MaquinaReturn.Nombre = MaquinaExistente.Nombre;
                    MaquinaReturn.NotariaId = MaquinaExistente.NotariaId;
                    MaquinaReturn.TipoMaquina = MaquinaExistente.TipoMaquina;
                    ConvenioRNEC convenioRNEC = (await _ConvenioRNECRepositorio.Obtener(x => x.NotariaId == MaquinaExistente.NotariaId).ConfigureAwait(false)).FirstOrDefault();
                    MaquinaReturn.IdCliente = convenioRNEC.IdCliente;
                    MaquinaReturn.IdConvenio = convenioRNEC.Convenio;
                    MaquinaReturn.IdOficina = convenioRNEC.IdOficina;
                    MaquinaReturn.EsvalidoFinal = true;
                }
                else
                {


                var Maquina = MaquinaDto.Adaptar<Maquina>();
                Maquina.UsuarioCreacion = MaquinaDto.UserName;
                Maquina.UsuarioModificacion = MaquinaDto.UserName;
                _MaquinaRepositorio.Agregar(Maquina);
                _MaquinaRepositorio.UnidadDeTrabajo.Commit();


               // InformationModel informationModel = new InformationModel(""
               //, PoliticaCapaEnum.Aplicacion.ToString()
               //, this.GetType().Name
               //, "CrearMaquina"
               //, "Maquinas", Maquina.MaquinaId.ToString(), JsonConvert.SerializeObject(MaquinaDto), MaquinaDto.UserName);
               // IdentificacionEquipo identificacionEquipo = new IdentificacionEquipo(MaquinaDto.MAC, MaquinaDto.DireccionIP, MaquinaDto.MAC);
               // SerilogFactory.Create().LogInformation(informationModel, identificacionEquipo, "");

                long MaquinaId = Maquina.MaquinaId;
                if (MaquinaId > 0)
                {
                    Maquina MaquinaBD = (await _MaquinaRepositorio.Obtener(x => x.MaquinaId == MaquinaId).ConfigureAwait(false)).FirstOrDefault();
                    MaquinaReturn.MaquinaId = MaquinaBD.MaquinaId;
                    MaquinaReturn.DireccionIP = MaquinaBD.DireccionIP;
                    MaquinaReturn.MAC = MaquinaBD.MAC;
                    MaquinaReturn.Nombre = MaquinaBD.Nombre;
                    MaquinaReturn.NotariaId = MaquinaBD.NotariaId;
                    MaquinaReturn.TipoMaquina = MaquinaBD.TipoMaquina;
                    ConvenioRNEC convenioRNEC = (await _ConvenioRNECRepositorio.Obtener(x => x.NotariaId == MaquinaBD.NotariaId).ConfigureAwait(false)).FirstOrDefault();
                    MaquinaReturn.IdCliente = convenioRNEC.IdCliente;
                    MaquinaReturn.IdConvenio = convenioRNEC.Convenio;
                    MaquinaReturn.IdOficina = convenioRNEC.IdOficina;
                    MaquinaReturn.EsvalidoFinal = true;
                }
                }
            }
            return MaquinaReturn;
        }

        
        #endregion

        #region Negocio
        public async Task<PaginableResponse<MaquinaConfiguracionReturnDTO>> ObtenerConfiguracionesUsuario(ConfiguracionesNotariaRequestDTO configDto)
        {
            if (configDto.NotariaId > 0 || !string.IsNullOrWhiteSpace(configDto.CorreoUsuario))
            {
                List<Maquina> maquinas;
                maquinas = (await _MaquinaRepositorio.Obtener(m => m.NotariaId == (configDto.NotariaId > 0 ? configDto.NotariaId : m.NotariaId) &&
                 m.UsuarioModificacion == (!string.IsNullOrWhiteSpace(configDto.CorreoUsuario) ? configDto.CorreoUsuario : m.UsuarioModificacion)
                )).ToList();

                return new PaginableResponse<MaquinaConfiguracionReturnDTO>
                {
                    Data = maquinas.Select(m => m.Adaptar<MaquinaConfiguracionReturnDTO>()),
                    Pages = (int)Math.Ceiling(maquinas.Count / (decimal)configDto.RegistrosPagina),
                    TotalRows = maquinas.Count
                };
            }
            else
                return new PaginableResponse<MaquinaConfiguracionReturnDTO>();
        }

        private async Task<MaquinaDTO> ValidarRequestCreacionMaquina(MaquinaCreateDTO MaquinaDto)
        {
            MaquinaDTO respuesta = new MaquinaDTO();
            try
            {
                #region Validación Negocio

                bool esNotariaActiva = await _MaquinaRepositorio.NotariaActiva(MaquinaDto.NotariaId);
                if (!esNotariaActiva)
                {
                    respuesta.Mensaje = NOTARIAINCORRECTA;
                    respuesta.MaquinaExiste = true;
                    return respuesta;

                }

                //var MacMaquinaExistente = (await _MaquinaRepositorio.Obtener(x => x.IsDeleted == false && x.MAC.Trim() == MaquinaDto.MAC.Trim() && x.NotariaId == MaquinaDto.NotariaId).ConfigureAwait(false)).Any();
                //if (MacMaquinaExistente)
                //{
                //    respuesta.Mensaje = string.Empty;
                //    respuesta.MaquinaExiste = true;
                //    respuesta.EsValido = false;
                //    respuesta.EsvalidoFinal = false;
                //    return respuesta;
                //}

                #endregion
            }
            catch (Exception)
            {
                respuesta.Mensaje = ERRORSISTEMA;
                return respuesta;
            }
            respuesta.EsValido = true;
            return respuesta;
        }

        public async Task<MaquinaConfiguracionReturnDTO> ConsultarMaquina(string mac)
        {
            var maquina = (await _MaquinaRepositorio.GetOneAsync(m => m.MAC == mac));
            return maquina.Adaptar<MaquinaConfiguracionReturnDTO>();
        }


        #endregion
    }
}
