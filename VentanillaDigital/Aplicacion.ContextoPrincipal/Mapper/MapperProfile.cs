using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using AutoMapper;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo.Entidad;
using System;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Infraestructura.Transversal.Models;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Dominio.ContextoPrincipal.Vista;
using GenericExtensions;
using Newtonsoft.Json;
using Dominio.ContextoPrincipal.Entidad;

namespace Aplicacion.ContextoPrincipal.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<NewRegisterDTO, EntidadBase>()
           .ForMember(dest => dest.UsuarioCreacion, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UsuarioModificacion, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => DateTime.Now)).IncludeAllDerived();


            CreateMap<EditRegisterDTO, EntidadBase>()
            .ForMember(dest => dest.FechaCreacion, opt => opt.UseDestinationValue())
            .ForMember(dest => dest.UsuarioCreacion, opt => opt.UseDestinationValue())
           .ForMember(dest => dest.UsuarioModificacion, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom<DateTime>(src => DateTime.Now)).IncludeAllDerived();


            CreateMap<Ubicacion, UbicacionReturnDTO>().PreserveReferences().ReverseMap();

            CreateMap<Categoria, CategoriaReturnDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<TipoTramite, TipoTramiteReturnDTO>()
                .PreserveReferences().ReverseMap();  
            
            CreateMap<Tramite, TramiteReturnDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<Maquina, MaquinaDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<MaquinaDTO, Maquina>()
                .PreserveReferences().ReverseMap();

            CreateMap<Maquina, MaquinaConfiguracionReturnDTO>()
                .ForMember(dest => dest.CorreoUsuario, opt => opt.MapFrom(src => src.UsuarioCreacion))
                .PreserveReferences().ReverseMap();

            CreateMap<TipoIdentificacion, TipoIdentificacionReturnDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<Notaria, NotariaForReturn>()
                .ForMember(dest => dest.NotariaNombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion.ToString()))
                .PreserveReferences().ReverseMap();

            #region NotariaUsuario

            CreateMap<NotariaUsuarioCreateDTO, NotariaUsuarios>()
                .ForMember(dest => dest.UsuariosId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UsuarioEmail))
                .PreserveReferences().ReverseMap();

            CreateMap<PersonaCreateOrUpdateDTO, Persona>()
                .PreserveReferences().ReverseMap();

            CreateMap<PersonaDatosDTO, PersonaDatos>()
                .PreserveReferences().ReverseMap();

            CreateMap<NotariaClienteDTO, NotariaCliente>()
                .ForMember(dest => dest.NotariaId, opt => opt.MapFrom(src => src.NotariaId))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .PreserveReferences().ReverseMap();


            #endregion

            #region Mapper Compareciente

            CreateMap<ComparecienteCreateDTO, Compareciente>()
                .PreserveReferences().ReverseMap();

            CreateMap<Documento, MetadataArchivo>()
                .ForMember(dest => dest.Tamanio, opt => opt.MapFrom(src => src.Tamano))
                .PreserveReferences().ReverseMap();

            CreateMap<Documento, Archivo>()
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Imagen))
                .PreserveReferences().ReverseMap();

            CreateMap<TramiteComparecienteReturnDTO, Compareciente>()
                .PreserveReferences().ReverseMap();

            CreateMap<ComparecienteCreateDTO, Persona>()
                .ForMember(dest => dest.NumeroCelular, opt => opt.MapFrom(src => src.Celular))
                .ForMember(dest => dest.TipoIdentificacionId, opt => opt.MapFrom(src => src.TipoDocumentoId))
                .ForMember(dest => dest.UsuarioCreacion, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UsuarioModificacion, opt => opt.MapFrom(src => src.UserName))
                .PreserveReferences().ReverseMap();

            CreateMap<Compareciente, ComparecienteReturnDTO>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Persona.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Persona.Apellidos))
                .ForMember(dest => dest.NumeroIdentificacion, opt => opt.MapFrom(src => src.Persona.NumeroDocumento))
                .ForMember(dest => dest.Foto, opt => opt.MapFrom(src => src.Foto.Archivo.Contenido))
                .ForMember(dest => dest.Firma, opt => opt.MapFrom(src => src.Firma.Archivo.Contenido))
                .ForMember(dest => dest.TipoIdentificacion, opt => opt.MapFrom(src => src.Persona.TipoIdentificacion.Adaptar<TipoIdentificacionReturnDTO>()))
              .PreserveReferences().ReverseMap();
            #endregion

            #region Mapper Tramite
            CreateMap<TramiteCreateDTO, Tramite>()
                .PreserveReferences();

            CreateMap<TramiteEditDTO, Tramite>()
                .PreserveReferences();

            CreateMap<MaquinaCreateDTO, Maquina>()
                .PreserveReferences().ReverseMap();

            CreateMap<NotarioCreateDTO, Notario>()
                .PreserveReferences().ReverseMap();

            #endregion

            CreateMap<TramitesPendientes, TramitePendienteReturnDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoTramite, opt => opt.MapFrom(src => src.TipoTramite))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Comparecientes, opt => opt.MapFrom(src => src.Comparecientes))
                .ForMember(dest => dest.DocCompareciente, opt => opt.MapFrom(src => src.DocCompareciente));

            CreateMap<DefinicionFiltroSimple, DefinicionFiltro>()
                .PreserveReferences().ReverseMap();

            CreateMap<RespuestaProcedimientoViewModel, ListaTramitePendienteReturnDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<RespuestaProcedimientoViewModel, ListaTramitePortalVirtualReturnDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<ConvenioRNEC, ConfiguracionRNECDTO>()
                .ForMember(dest => dest.ConvenioRNEC, opt => opt.MapFrom(src => new Guid(src.Convenio)))
                .ForMember(dest => dest.ClienteRNECId, opt => opt.MapFrom(src => src.IdCliente));

            CreateMap<Notaria, ConfiguracionRNECDTO>()
                .ForMember(dest => dest.OficinaRNEC, opt => opt.MapFrom(src => src.Nombre));

            #region Mapper Plantillas Actas
            CreateMap<PlantillaActaCreateDTO, PlantillaActa>()
                .PreserveReferences().ReverseMap();
            CreateMap<PlantillaActaEditDTO, PlantillaActa>()
                .PreserveReferences().ReverseMap();
            CreateMap<PlantillaActa, PlantillaActaReturnDTO>()
                .PreserveReferences().ReverseMap();
            CreateMap<PlantillaActa, PlantillaActaFullReturnDTO>()
                .PreserveReferences().ReverseMap();

            #endregion

            #region Mapper Acta Notarial
            CreateMap<FirmaActaNotarialReturnDTO, AutorizacionTramitesResponseDTO>();
            CreateMap<ActaCreateDTO,ActaCreateToPlantilla>()
                .ForMember(dest => dest.TramiteId, opt => opt.MapFrom(src => src.TramiteId.ToString()))
                .PreserveReferences().ReverseMap(); 

            CreateMap<DatosTramiteResumen, ActaResumen>()
                .ForMember(dest => dest.TramiteId, opt => opt.MapFrom(src => src.TramiteId))
                .ForMember(dest => dest.TramiteFecha, opt => opt.MapFrom(src => src.TramiteFecha))
                .ForMember(dest => dest.NotariaNombre, opt => opt.MapFrom(src => src.NotariaNombre))
                .ForMember(dest => dest.TipoTramiteId, opt => opt.MapFrom(src => src.CodigoTramite))
                .ForMember(dest => dest.TipoTramiteNombre, opt => opt.MapFrom(src => src.TipoTramiteNombre))
                .ForMember(dest => dest.DatosAdicionales, opt => opt.MapFrom(src => src.DatosAdicionales))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

            CreateMap<DatosComparecientesResumen, ComparecienteResumen>()
             .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
             .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
             .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
             .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NumeroDocumento))
             .ForMember(dest => dest.SinBiometria, opt => opt.MapFrom(src => src.SinBiometria))
             .ForMember(dest => dest.MotivoSinBiometria, opt => opt.MapFrom(src => src.MotivoSinBiometria))
             .ForMember(dest => dest.Foto, opt => opt.MapFrom(src => src.Foto));

            #endregion

            CreateMap<Notario, NotarioReturnDTO>()
                .ForMember(dest => dest.NotarioId, opt => opt.MapFrom(src => src.NotarioId))
                .ForMember(dest => dest.NotarioNombre, opt => opt.MapFrom(src => $"{src.NotariaUsuarios.Persona.Nombres} {src.NotariaUsuarios.Persona.Apellidos}"));

            #region Mapper Certificado
            CreateMap<CertificadoCreateDTO, Certificado>()
                .ForMember(dest => dest.Datos, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UserId))
              .PreserveReferences().ReverseMap();

            CreateMap<TramitePortalVirtualCiudadanoDTO, TramitesPortalVirtual>()
                .ForMember(dest => dest.NotariaId, opt => opt.MapFrom(src => src.NotariaId))
                .ForMember(dest => dest.TipoTramiteVirtualId, opt => opt.MapFrom(src => src.TipoTramiteVirtualId))
                .ForMember(dest => dest.EstadoTramiteVirtualId, opt => opt.MapFrom(src => src.EstadoTramiteVirtualId))
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NumeroDocumento))
                .ForMember(dest => dest.TramiteVirtualID, opt => opt.MapFrom(src => src.TramiteVirtualID))
                .ForMember(dest => dest.TramiteVirtualGuid, opt => opt.MapFrom(src => src.TramiteVirtualGuid))
                .ForMember(dest => dest.CUANDI, opt => opt.MapFrom(src => src.CUANDI))
                .ForMember(dest => dest.DatosAdicionales, opt => opt.MapFrom(src => src.DatosAdicional))
                .ForMember(dest => dest.ActoPrincipalId, opt => opt.MapFrom(src => src.ActoPrincipalId))
                .PreserveReferences().ReverseMap();

            CreateMap<ArchivoRequest, ArchivosDTO>()
                .PreserveReferences().ReverseMap();
            
            CreateMap<NotariaUsuarios, CertificadoSelectedDTO>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuariosId))
                .ForMember(dest => dest.UsuarioCertificado, opt => opt.MapFrom(src => src.Notario.UsuarioCertificado))
                .ForMember(dest => dest.CertificadoId, opt => opt.MapFrom(src => src.Notario.Certificadoid))
              .PreserveReferences().ReverseMap();
            #endregion


        }
    }
}
